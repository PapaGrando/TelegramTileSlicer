using TelegramCropper.Jobs;

namespace TelegramCropper.Interfaces
{
    public interface IChatJob : IDisposable
    {
        public event EventHandler LifeTimeElapsed;
        public IReadOnlyCollection<IFilterJob> FiltersQueue { get; }
        bool IsBusy { get; }
        TileJob TileTask { get; }
        void AddFilter(IFilterJob filterAct);
        Task<Stream> ProcessTask(Stream imageStream, string nameId);
    }
}
