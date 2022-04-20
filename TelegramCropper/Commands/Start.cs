using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Commands
{
    public class Start : BaseCommand
    {
        public const string EXAMPLE_PATH = "Images/example.png";
        public override string CommandHelp => "Hi! \n" +
                "Im tile-slicer bot. I can slice tilesets and applies some filters" +
                "\n\nUse /help for info";

        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatTask> chatsList,
            CommandData commandData, Message message)
        {
            if (System.IO.File.Exists(EXAMPLE_PATH))
                using (var example = new FileStream(EXAMPLE_PATH, FileMode.Open))
                {
                    var photo = new InputOnlineFile(example, "example.png");
                    await botClient.SendPhotoAsync(message.Chat!, photo, caption: CommandHelp);
                }
            else
                await botClient.SendTextMessageAsync(message.Chat!, CommandHelp);

            return true;
        }
    }
}
