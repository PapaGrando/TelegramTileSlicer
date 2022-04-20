using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Exceptions;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Commands
{
    public class Tile : BaseCommand
    {
        public override string CommandHelp => "Sets the size of the tile, into which the image should be cut.\n" +
            "Usage:\n\n" +
            "/tile *Height(pixels) *Width(pixels)\n\n" +
            "Example:\n" +
            "/tile 16 16\n\n" +
            "/tile 0 0 disabling tiling ";

        private const int _MIN_ARGS_COUNT = 1;
        private const int _MAX_ARGS_COUNT = 2;

        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatTask> chatList,
            CommandData commandData, Message message)
        {
            var task = chatList.TryGetChat(message.Chat.Id);

            if (task == null)
                throw new CommandArgumentsException($"Use /new to create task\n {CommandHelp}");

            var args = commandData.Args;
            (int Height, int Width) size;

            if (!CheckAndCorrectArgs(args, out size))
                throw new CommandArgumentsException($"Icorrect tile size args\n{CommandHelp}");

            try
            {
                task.TileTask.Sizes = size;
            }
            catch (TileTaskArgException ex)
            {
                throw new CommandArgumentsException(ex.Message);
            }

            await botClient.SendTextMessageAsync(message.Chat.Id, size != (0, 0) ?
                $"Tile size setted. H:{task.TileTask.Height}; W:{task.TileTask.Width}" :
                "Tile slicing disabled");

            return true;
        }

        private bool CheckAndCorrectArgs(string[] args, out (int Height, int Width) size)
        {
            size = (0, 0);
            int w;

            if (args.Length < _MIN_ARGS_COUNT ||
                args.Length > _MAX_ARGS_COUNT)

                return false;

            if (!int.TryParse(args[0], out int h))
                return false;

            if (args.Length == 1)
                w = h;
            else if (!int.TryParse(args[0], out w))
                return false;

            size.Height = h;
            size.Width = w;

            return true;
        }
    }
}
