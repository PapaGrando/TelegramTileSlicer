using System.Collections.Concurrent;
using TelegramCropper.Interfaces;
using TelegramCropper.Tasks;

namespace TelegramCropper.Repo
{
    public class ChatTaskRepo : IChatRepo<IChatTask>
    {
        public IReadOnlyDictionary<long, IChatTask> ChatTasks => _chatTasks;
        private ConcurrentDictionary<long, IChatTask> _chatTasks;
        private int _taskLifetimeSec = 300;
        private int _imageProcessTimeoutSec = 60;

        public ChatTaskRepo(int taskLifetime, int processTimeout)
        {
            _taskLifetimeSec = taskLifetime > 0 ? taskLifetime : _taskLifetimeSec;
            _imageProcessTimeoutSec = processTimeout > 0 ? processTimeout : _imageProcessTimeoutSec;

            _chatTasks = new ConcurrentDictionary<long, IChatTask>();
        }

        public IChatTask AddOrUpdateChat(long id)
        {
            var ct = new ChatTask(_taskLifetimeSec, _imageProcessTimeoutSec);

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

        public IChatTask? TryGetChat(long id) =>
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