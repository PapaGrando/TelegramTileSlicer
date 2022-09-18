using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using TelegramCropper.Exceptions;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Handlers
{
    public class DocumentHandler : BaseHandler
    {
        public override async Task Handle(ITelegramBotClient botClient, Update update, 
            IChatRepo<IChatJob> chats, CancellationToken cancellationToken)
        {
            var chat = update.Message.Chat;
            var chatTask = chats.TryGetChat(chat.Id);

            if (!await ChecksForDocument(botClient, chat.Id, chatTask, update.Message.Document.MimeType))
                return;

            var fileInfo = await botClient.GetFileAsync(update.Message.Document.FileId);

            if (fileInfo.FileSize > 4194304) // 4MB
            {
                await botClient.SendTextMessageAsync(chat, "Size limit - 4Mb");
                return;
            }

            try
            {
                using var fileStr = new MemoryStream();

                await botClient.DownloadFileAsync(fileInfo.FilePath, fileStr);

                using Stream outstream = await chatTask.ProcessTask(fileStr, chat.Id.ToString());
                outstream.Position = 0;
                var iof = new InputOnlineFile(outstream, "out.zip");

                await botClient.SendDocumentAsync(update.Message.Chat, iof);
            }
            catch (ChatTaskException ex)
            {
                await botClient.SendTextMessageAsync(chat, ex.Message);
            }
        }

        public async static Task<bool> ChecksForDocument(ITelegramBotClient bot,
            long chatId, IChatJob? chatTask, string mime)
        {
            if (chatTask is null)
            {
                await bot.SendTextMessageAsync(chatId, "First create config by /new before upload\n\n /help");
                return false;
            }

            if (chatTask.IsBusy)
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
    }
}
