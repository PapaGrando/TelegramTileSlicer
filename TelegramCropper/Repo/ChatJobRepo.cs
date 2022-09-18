using System.Collections.Concurrent;
using TelegramCropper.Interfaces;
using TelegramCropper.Jobs;

namespace TelegramCropper.Repo
{
    public class ChatJobRepo : IChatRepo<IChatJob>
    {
        public IReadOnlyDictionary<long, IChatJob> ChatTasks => _chatTasks;
        private ConcurrentDictionary<long, IChatJob> _chatTasks;
        private int _taskLifetimeSec = 300;
        private int _imageProcessTimeoutSec = 60;

        public ChatJobRepo(int taskLifetime, int processTimeout)
        {
            _taskLifetimeSec = taskLifetime > 0 ? taskLifetime : _taskLifetimeSec;
            _imageProcessTimeoutSec = processTimeout > 0 ? processTimeout : _imageProcessTimeoutSec;

            _chatTasks = new ConcurrentDictionary<long, IChatJob>();
        }

        public IChatJob AddOrUpdateChat(long id)
        {
            var ct = new ChatJob(_taskLifetimeSec, _imageProcessTimeoutSec);

            ct.LifeTimeElapsed += (sender, e) => DeleteChat(id);

            _chatTasks.AddOrUpdate(id, ct, (key, oldVal) =>
             {
                 oldVal.Dispose();
                 return ct;
             });

            return ct;
        }

        public bool HasChat(long id) =>
            _chatTasks.ContainsKey(id);

        public IChatJob? TryGetChat(long id) =>
            _chatTasks.TryGetValue(id, out var chatTask) ? chatTask : null;

        public bool DeleteChat(long id)
        {
            if (_chatTasks.Remove(id, out var c))
            {
                c.Dispose();
                return true;
            }
            return false;
        }
    }
}