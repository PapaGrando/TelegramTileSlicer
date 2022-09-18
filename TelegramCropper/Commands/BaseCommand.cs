using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Commands
{
    abstract public class BaseCommand : ICommand
    {
        public abstract string CommandHelp { get; }
        public virtual async Task<bool> Run(ITelegramBotClient botClient,
            IChatRepo<IChatJob> chatList,
            CommandData commandData, Message message)
        {
            await botClient.SendTextMessageAsync(message.Chat, CommandHelp);
            return true;
        }
    }
}
