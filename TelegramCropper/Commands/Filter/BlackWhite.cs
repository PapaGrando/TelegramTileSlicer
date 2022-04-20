using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;
using TelegramCropper;
using TelegramCropper.Tasks;

namespace TelegramCropper.Commands.Filter
{
    public class BlackWhite : FilterBaseCommand
    {
        public override string CommandHelp => "Makes picture Black and White";
        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatTask> chatsList,
            CommandData commandData, Message message)
        {
            await TryAddFilterOrShowHelp(botClient, chatsList, message,
                new FilterTask("Black and white filter", (x) => x.BlackWhite()));

            return true;
        }
    }
}
