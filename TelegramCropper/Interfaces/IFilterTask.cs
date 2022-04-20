using SixLabors.ImageSharp.Processing;

namespace TelegramCropper.Interfaces
{
    public interface IFilterTask
    {
        string Name { get; }
        Action<IImageProcessingContext> FilterAction { get; }
    }
}