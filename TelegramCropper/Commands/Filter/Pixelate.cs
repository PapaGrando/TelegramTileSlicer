using SixLabors.ImageSharp.Processing;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;
using TelegramCropper.Jobs;

namespace TelegramCropper.Commands.Filter
{
    public class Pixelate : FilterBaseCommand
    {
        public override string CommandHelp => "Pixelates picture";
        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatJob> chatsList,
            CommandData commandData, Message message)
        {
            await TryAddFilterOrShowHelp(botClient, chatsList, message,
                new FilterJob("Pixelate filter", (x) => x.Pixelate()));

            return true;
        }
    }
}
