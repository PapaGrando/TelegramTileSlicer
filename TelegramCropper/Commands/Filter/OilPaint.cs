using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;
using TelegramCropper.Jobs;

namespace TelegramCropper.Commands.Filter
{
    public class OilPaint : FilterBaseCommand
    {
        public override string CommandHelp => "Oil paint effect";
        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatJob> chatsList,
            CommandData commandData, Message message)
        {
            await TryAddFilterOrShowHelp(botClient, chatsList, message,
                new FilterJob("Oil filter", (x) => x.OilPaint()));

            return true;
        }
    }
}