using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Commands;
using TelegramCropper.Interfaces;

namespace TelegramCropper
{
    public static class Utils
    {
        public static CommandData ParseCommand(string message)
        {
            if (message == null ||
                message.Length < 2 || //with slash
                !message.StartsWith('/'))
                return null;

            var mesArr = message.Split(' ');
            CommandData commandData = null;

            if (mesArr.Length > 0)
                commandData = new CommandData()
                {
                    CommnadName = mesArr[0].TrimStart('/'),
                    Args = Array.Empty<string>()
                };

            if (mesArr.Length > 1)
                commandData.Args = mesArr.Skip(1).ToArray();

            return commandData;
        }

        public static void ConfigureCommands(ITelegramBotClient bot)
        {
            BotCommand[] coms = new[]
            {
                new BotCommand() { Command = "start", Description = "Greetings!" },
                new BotCommand() { Command = "help", Description = "How it to use?" },
                new BotCommand() { Command = "new", Description = "New task, reset old" },
                new BotCommand() { Command = "status", Description = "Check status current task" },
                new BotCommand() { Command = "filters", Description = "Available filters" },
                new BotCommand() { Command = "credits", Description = "Author info" }
            };
            bot.SetMyCommandsAsync(coms);
        }

        public async static Task<bool> ChecksForDocument(ITelegramBotClient bot,
            long chatId, IChatTask? chatTask, string mime)
        {
            if (chatTask is null)
            {
                await bot.SendTextMessageAsync(chatId, "First create config by /new before upload\n\n /help");
                return false;
            }

            if (chatTask.IsProcessing)
            {
                await bot.SendTextMessageAsync(chatId, "Task processing... Wait");
                return false;
            }

            if (mime != "image/png")
            {
                await bot.SendTextMessageAsync(chatId, "Currently only .PNG is supported. :(");
                return false;
            }

            return true;
        }

        public static bool CheckConfig(Config conf)
        {
            if (conf.ApiKey.Length == 0)
            {
                Console.WriteLine("Paste the Api-key from BotFather into the config.json file");
                return false;
            }

            if (conf.MaxTaskProcessTimeoutSec < 1)
            {
                Console.WriteLine("MaxTaskProcessTimeoutSec must be greater than 1");
                return false;
            }

            if (conf.MaxTasksLifeTimeSec <= conf.MaxTaskProcessTimeoutSec)
            {
                Console.WriteLine("MaxTasksLifeTimeSec must be greater than MaxTaskProcessTimeoutSec!");
                return false;
            }

            return true;
        }
    }
}
