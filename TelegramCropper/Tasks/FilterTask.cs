using SixLabors.ImageSharp.Processing;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Tasks
{
    public record FilterTask : IFilterTask
    {
        public string Name { get; init; }
        public Action<IImageProcessingContext> FilterAction { get; init; }

        public FilterTask(string name, Action<IImageProcessingContext> filterAction)
        {
            Name = name;
            FilterAction = filterAction;
        }
    }
}
