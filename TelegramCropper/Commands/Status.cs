using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Commands
{
    public class Status : BaseCommand
    {
        public override string CommandHelp => "Displays current status of task.";
        public override async Task<bool> Run(ITelegramBotClient botClient, IChatRepo<IChatTask> chatsList,
            CommandData commandData, Message message)
        {
            var chatTask = chatsList.TryGetChat(message.Chat.Id);

            if (chatTask is null)
            {
                await botClient.SendTextMessageAsync(message.Chat!,
                    $"{CommandHelp}\n\nNo working task at the moment. Use /new for create. /help for info");
                return false;
            }
            await botClient.SendTextMessageAsync(message.Chat!,
                    MakeChatTaskInfo(chatTask));

            return true;
        }

        private string MakeChatTaskInfo(IChatTask task)
        {
            var sb = new StringBuilder();
            sb.Append("Task configuration:\n");
            sb.AppendLine("=========");
            sb.Append("Filters: (applied in sequence)\n");

            //filter info
            if (task.FiltersQueue.Any())
            {
                var i = 1;
                foreach (var f in task.FiltersQueue)
                {
                    sb.AppendLine($"{i}) - {f.Name}");
                    i++;
                }
            }
            else
                sb.AppendLine("No Filters");

            //tiling slice info
            sb.Append('\n');
            sb.AppendLine("Tile size:");
            sb.AppendLine(
                task.TileTask.Sizes == (0, 0) ? "No tiling" :
                $"Height: {task.TileTask.Height}\nWidth: {task.TileTask.Width}");

            sb.AppendLine("=========");
            //is Processing?
            sb.AppendLine(task.IsProcessing ? "Processing..." : "Waiting File...\n");
            sb.AppendLine("Use .png file. Max 4Mp, 4MB");
            sb.AppendLine("Max tiles - 500");

            return sb.ToString();
        }
    }
}
