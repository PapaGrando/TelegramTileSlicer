using SixLabors.ImageSharp.Processing;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;
using TelegramCropper.Jobs;

namespace TelegramCropper.Commands.Filter
{
    public class BlackWhite : FilterBaseCommand
    {
        public override string CommandHelp => "Makes picture Black and White";
        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatJob> chatsList,
            CommandData commandData, Message message)
        {
            await TryAddFilterOrShowHelp(botClient, chatsList, message,
                new FilterJob("Black and white filter", (x) => x.BlackWhite()));

            return true;
        }
    }
}
