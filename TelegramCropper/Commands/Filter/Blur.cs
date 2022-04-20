using SixLabors.ImageSharp.Processing;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;
using TelegramCropper;
using TelegramCropper.Tasks;

namespace TelegramCropper.Commands.Filter
{
    public class Blur : FilterBaseCommand
    {
        public override string CommandHelp => "Adds Gaussian blur to picture";
        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatTask> chatsList,
            CommandData commandData, Message message)
        {
            await TryAddFilterOrShowHelp(botClient, chatsList, message,
                new FilterTask("Gaussian blur filter", (x) => x.GaussianBlur()));

            return true;
        }
    }
}