using SixLabors.ImageSharp.Processing;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Jobs
{
    public record FilterJob : IFilterJob
    {
        public string Name { get; init; }
        public Action<IImageProcessingContext> FilterAction { get; init; }

        public FilterJob(string name, Action<IImageProcessingContext> filterAction)
        {
            Name = name;
            FilterAction = filterAction;
        }
    }
}
