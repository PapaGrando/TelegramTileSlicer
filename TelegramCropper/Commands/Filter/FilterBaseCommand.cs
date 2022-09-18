using Telegram.Bot;
using Telegram.Bot.Types;
using SixLabors.ImageSharp.Processing;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Commands.Filter
{
    public abstract class FilterBaseCommand : BaseCommand, IFilterCommand
    {
        protected async Task TryAddFilterOrShowHelp(ITelegramBotClient bot, IChatRepo<IChatJob> chatsList,
            Message message, IFilterJob todo)
        {
            var chattask = chatsList.TryGetChat(message.Chat.Id);
            if (chattask is null)
            {
                await bot.SendTextMessageAsync(message.Chat, CommandHelp);
                return;
            }

            chattask.AddFilter(todo);
            await bot.SendTextMessageAsync(message.Chat, $"{todo.Name} added to queue");
            return;
        }
    }
}
