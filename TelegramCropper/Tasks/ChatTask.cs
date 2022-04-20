using System.Collections.Concurrent;
using TelegramCropper.Exceptions;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Tasks
{
    public class ChatTask : IChatTask, IDisposable
    {
        public event EventHandler LifeTimeElapsed;
        public bool IsProcessing { get; private set; }
        public IReadOnlyCollection<IFilterTask> FiltersQueue => _filtersQueue;
        public TileTask TileTask
        {
            get
            {
                TimerReset(_taskLifetimeSec);
                return _tileTask;
            }
        }
        private TileTask _tileTask;
        private ConcurrentQueue<IFilterTask> _filtersQueue;
        private System.Timers.Timer _timer;
        private int _taskLifetimeSec;
        private int _imageProcessTimeoutSec;
        private bool disposedValue;

        public ChatTask(int lifeTimeSec, int imageProcessTimeout)
        {
            if (lifeTimeSec < 1 ||
                imageProcessTimeout < 1)
                throw new ChatTaskArgumentsException("Task life time and timeout must be MORE than 0");

            _taskLifetimeSec = lifeTimeSec;
            _imageProcessTimeoutSec = imageProcessTimeout;

            _timer = new System.Timers.Timer();
            _timer.Elapsed += (s, e) => LifeTimeElapsed?.Invoke(this, EventArgs.Empty);

            IsProcessing = false;
            _tileTask = new TileTask();
            _filtersQueue = new ConcurrentQueue<IFilterTask>();

            TimerReset(lifeTimeSec);
        }

        public async Task<Stream> ProcessTask(Stream imageStream, string nameID)
        {
            if (IsProcessing)
                throw new ChatTaskBusyException();

            IsProcessing = true;

            TimerReset(_taskLifetimeSec);

            using var cansTok = new CancellationTokenSource();
            using var imageProcess = new ImageProcess(nameID, cansTok.Token);
            try
            {
                var t = Task.Run(() => imageProcess.ProcessAction(imageStream, TileTask, _filtersQueue));

                if (await Task.WhenAny(t, Task.Delay(TimeSpan.FromSeconds(_imageProcessTimeoutSec))) != t)
                    cansTok.Cancel();

                return t.GetAwaiter().GetResult();
            }
            catch (ChatTaskTimeoutException ex)
            {
                cansTok.Cancel();
                throw ex;
            }
            finally
            {
                cansTok.Dispose();
                imageProcess.Dispose();
                IsProcessing = false;
            }
        }

        public void AddFilter(IFilterTask filterAct)
        {
            if (IsProcessing)
                throw new ChatTaskBusyException();

            _filtersQueue.Enqueue(filterAct);

            TimerReset(_taskLifetimeSec);
        }

        public void TimerReset(int newTime)
        {
            _timer.Stop();
            _timer.Interval = TimeSpan.FromSeconds(newTime).TotalMilliseconds;
            _timer.Start();
        }

        #region dispose
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue || !disposing)
                return;

            IsProcessing = false;

            _timer.Stop();
            _timer.Dispose();

            foreach (var d in LifeTimeElapsed.GetInvocationList())
                LifeTimeElapsed -= (EventHandler)d;

            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}