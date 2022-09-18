using SixLabors.ImageSharp.Processing;

namespace TelegramCropper.Interfaces
{
    public interface IFilterJob
    {
        string Name { get; }
        Action<IImageProcessingContext> FilterAction { get; }
    }
}