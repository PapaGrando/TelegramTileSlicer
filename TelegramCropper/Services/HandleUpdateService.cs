using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using TelegramCropper.Handlers;
using TelegramCropper.Interfaces;

namespace TelegramCropper.Services;

public class HandleUpdateService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HandleUpdateService> _logger;
    private readonly IChatRepo<IChatJob> _chats;
    private readonly ICommandsFactory _commandsFactory;

    public HandleUpdateService(ITelegramBotClient botClient,
        ILogger<HandleUpdateService> logger,
        IChatRepo<IChatJob> chats,
        ICommandsFactory commandsFactory)
    {
        _botClient = botClient;
        _logger = logger;
        _chats = chats;
        _commandsFactory = commandsFactory;
    }

    public async Task EchoAsync(Update update, CancellationToken cancellationToken)
    {
        try
        {
#pragma warning disable CS4014, CS8602

            if (update.Message.Document is not null)
                Task.Run(() => new DocumentHandler().Handle(_botClient, update, _chats, cancellationToken));

            else if (update.Message.Text is not null)
                Task.Run(() => new MessageHandler(_commandsFactory)
                .Handle(_botClient, update, _chats, cancellationToken));

#pragma warning restore CS4014, CS8602
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(exception);
        }
    }

    public Task HandleErrorAsync(Exception exception)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
        return Task.CompletedTask;
    }
}
