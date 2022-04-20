namespace TelegramCropper.Interfaces
{
    public interface IChatImage
    {
        int Width { get; init; }
        int Height { get; init; }
        string FileExt { get; init; }
        string FilePath { get; init; }
    }
}
