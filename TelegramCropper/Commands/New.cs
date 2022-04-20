using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Commands
{
    public class New : BaseCommand
    {
        public override string CommandHelp =>
            "/new\n\n" +
            "Creates settings for new tileset\n" +
            "Old settings will resets";

        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatTask> chatsList,
            CommandData commandData, Message message)
        {
            var ct = chatsList.TryGetChat(message.Chat.Id);

            if (ct is not null)
                await botClient.SendTextMessageAsync(message.Chat!, "Old task will be removed");

            var chatTask = chatsList.AddOrUpdateChat(message.Chat.Id);
            await botClient.SendTextMessageAsync(message.Chat!, "New task created. " +
                    "Use commands /tile, avariable filters (/filters), /status for checking current task");

            return true;
        }
    }
}