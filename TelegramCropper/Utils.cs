using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Commands;

namespace TelegramCropper
{
    public static class Utils
    {
        public static CommandData? ParseCommand(this Message msg)
        {
            var text = msg.Text;
            if (text == null ||
                text.Length < 2 || //with slash
                !text.StartsWith('/'))
                return null;

            var mesArr = text.Split(' ');
            CommandData? commandData = null;

            if (mesArr.Length > 0)
            {
                commandData = new CommandData()
                {
                    CommnadName = mesArr[0].TrimStart('/'),
                    Args = Array.Empty<string>()
                };

                if (mesArr.Length > 1)
                    commandData.Args = mesArr.Skip(1).ToArray();
            }
            return commandData;
        }

        public static void ConfigureCommands(this TelegramBotClient bot)
        {
            BotCommand[] coms = new[]
            {
                new BotCommand() { Command = "start",   Description = "Greetings!" },
                new BotCommand() { Command = "help",    Description = "How it to use?" },
                new BotCommand() { Command = "new",     Description = "New task, reset old" },
                new BotCommand() { Command = "status",  Description = "Check status current task" },
                new BotCommand() { Command = "filters", Description = "Available filters" },
                new BotCommand() { Command = "credits", Description = "Author info" }
            };
            bot.SetMyCommandsAsync(coms);
        }

        public static void CheckConfig(this Config conf)
        {
            if (conf.ApiKey.Length == 0)
                throw new ArgumentException("Paste the Api-key from BotFather into the appsettings.json file");

            if (conf.MaxTaskProcessTimeoutSec < 1)
                throw new ArgumentException("MaxTaskProcessTimeoutSec must be greater than 1");

            if (conf.MaxTasksLifeTimeSec <= conf.MaxTaskProcessTimeoutSec)
                throw new ArgumentException("MaxTasksLifeTimeSec must be greater than MaxTaskProcessTimeoutSec!");

            if (conf.HostAddress.Length == 0)
                throw new ArgumentException("Paste the HostAdress from Ngrok(or your service) into the appsettings.json file");
        }
    }
}
