using SixLabors.ImageSharp.Processing;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;
using TelegramCropper.Tasks;

namespace TelegramCropper.Commands.Filter
{
    public class Pixelate : FilterBaseCommand
    {
        public override string CommandHelp => "Pixelates picture";
        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatTask> chatsList,
            CommandData commandData, Message message)
        {
            await TryAddFilterOrShowHelp(botClient, chatsList, message,
                new FilterTask("Pixelate filter", (x) => x.Pixelate()));

            return true;
        }
    }
}
