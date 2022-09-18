using SixLabors.ImageSharp.Processing;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;
using TelegramCropper.Jobs;

namespace TelegramCropper.Commands.Filter
{
    public class Sepia : FilterBaseCommand
    {
        public override string CommandHelp => "Adds Sepia effect to picture";
        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatJob> chatsList,
            CommandData commandData, Message message)
        {
            await TryAddFilterOrShowHelp(botClient, chatsList, message,
                 new FilterJob("Sepia filter", (x) => x.Sepia()));

            return true;
        }
    }
}