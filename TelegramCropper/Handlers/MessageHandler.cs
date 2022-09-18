using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Exceptions;
using TelegramCropper.Factory;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Handlers
{
    internal class MessageHandler : BaseHandler
    {
        private readonly ICommandsFactory _commandsFactory;

        public MessageHandler(ICommandsFactory commandsFactory)
        {
            _commandsFactory = commandsFactory;
        }

        public async override Task Handle(
            ITelegramBotClient botClient, Update update, 
            IChatRepo<IChatJob> chats, CancellationToken cancellationToken )
        {
            var message = update.Message;

            if (message is null)
                return;

            var comAndArgs = message.ParseCommand();

            if (comAndArgs == null)
                return;

            var command = _commandsFactory.GetCommand(comAndArgs.CommnadName);

            if (command == null)
                return;

            try
            {
                await command.Run(botClient,
                    chats, comAndArgs, message);
            }
            catch (BaseCommandException ex)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, ex.Message);
            }
        }
    }
}
