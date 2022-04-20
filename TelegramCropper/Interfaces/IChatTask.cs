using TelegramCropper.Tasks;

namespace TelegramCropper.Interfaces
{
    public interface IChatTask : IDisposable
    {
        public event EventHandler LifeTimeElapsed;
        public IReadOnlyCollection<IFilterTask> FiltersQueue { get; }
        bool IsProcessing { get; }
        TileTask TileTask { get; }
        void AddFilter(IFilterTask filterAct);
        Task<Stream> ProcessTask(Stream imageStream, string nameId);
    }
}
