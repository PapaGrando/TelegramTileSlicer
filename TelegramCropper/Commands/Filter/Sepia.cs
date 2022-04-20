using SixLabors.ImageSharp.Processing;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;
using TelegramCropper.Tasks;

namespace TelegramCropper.Commands.Filter
{
    public class Sepia : FilterBaseCommand
    {
        public override string CommandHelp => "Adds Sepia effect to picture";
        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatTask> chatsList,
            CommandData commandData, Message message)
        {
            await TryAddFilterOrShowHelp(botClient, chatsList, message,
                 new FilterTask("Sepia filter", (x) => x.Sepia()));

            return true;
        }
    }
}