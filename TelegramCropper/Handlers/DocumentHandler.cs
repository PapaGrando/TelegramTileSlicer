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
            IChatRepo<IChatTask> chats, CancellationToken cancellationToken)
        {
            var chat = update.Message.Chat;
            var chatTask = chats.TryGetChat(chat.Id);

            if (!await Utils.ChecksForDocument(botClient, chat.Id, chatTask, update.Message.Document.MimeType))
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
            //unknown ex
            catch (Exception ex)
            {
                Console.WriteLine($"\nImage Operation ERROR - {ex}");
            }
        }
    }
}
