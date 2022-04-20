using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Exceptions;
using TelegramCropper.Factory;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Handlers
{
    internal class MessageHandler : BaseHandler
    {
        public async override Task Handle(ITelegramBotClient botClient, Update update, 
            IChatRepo<IChatTask> chats, CancellationToken cancellationToken)
        {

            var message = update.Message;
            var comAndArgs = Utils.ParseCommand(message.Text);

            if (comAndArgs == null)
                return;

            var command = CommandsFactory.GetCommand(comAndArgs.CommnadName);

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
            //unknown ex
            catch (Exception ex)
            {
                Console.WriteLine($"\nMesage ERROR - {ex}");
            }
        }
    }
}
