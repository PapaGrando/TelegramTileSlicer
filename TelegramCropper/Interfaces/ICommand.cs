using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Commands;

namespace TelegramCropper.Interfaces
{
    public interface ICommand
    {
        /// <summary>
        /// String, storing text of command help
        /// </summary>
        string CommandHelp { get; }

        /// <summary>
        /// Command Invoke
        /// </summary>
        /// <returns>Success or not/returns>
        /// <exception cref="BaseCommandException"/>
        Task<bool> Run(ITelegramBotClient botClient,
            IChatRepo<IChatJob> chatList,
            CommandData commandData, Message message);
    }
}
