namespace TelegramCropper.Interfaces
{
    public interface IChatRepo<T>
    {
        IReadOnlyDictionary<long, T> ChatTasks { get; }
        bool HasChat(long id);
        /// <returns>IChatTask if exist, null if not</returns>
        T? TryGetChat(long id);
        T AddOrUpdateChat(long id);
        bool DeleteChat(long id);
    }
}