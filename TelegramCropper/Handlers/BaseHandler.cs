using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Handlers
{
    public abstract class BaseHandler
    {
        public abstract Task Handle(ITelegramBotClient botClient, Update update, 
            IChatRepo<IChatTask> chats, CancellationToken cancellationToken);
    }
}
