using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using System.Text.Json;
using System.Text.Json.Serialization;
using TelegramCropper.Factory;
using TelegramCropper.Interfaces;
using TelegramCropper;
using TelegramCropper.Repo;
using TelegramCropper.Handlers;
using Telegram.Bot.Types.Enums;

//StartUp configuration (void Main())

Console.WriteLine("Init...");

var config = JsonSerializer.Deserialize<Config>(System.IO.File.ReadAllText("config.json"));

if (!Utils.CheckConfig(config))
{
    Console.WriteLine("Error in Config");
    Console.ReadKey();
    return;
}

ITelegramBotClient bot = new TelegramBotClient(config.ApiKey);
Utils.ConfigureCommands(bot);

IChatRepo<IChatTask> chats = new ChatTaskRepo(config.MaxTasksLifeTimeSec,
    config.MaxTaskProcessTimeoutSec);
CommandsFactory.Cache();

JsonSerializerOptions jsonOpt = new JsonSerializerOptions()
{ DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

Console.WriteLine("Started - " + bot.GetMeAsync().Result.FirstName);

var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;
var receiverOptions = new ReceiverOptions
{ AllowedUpdates = new[] { UpdateType.Message } };

bot.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken
);
//Infinite wait
await Task.Delay(Timeout.Infinite).ConfigureAwait(false);

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    Console.WriteLine(JsonSerializer.Serialize(update.Message, jsonOpt));

    if (update.Message.Document is not null)
        Task.Run(() => new DocumentHandler().Handle(botClient, update, chats, cancellationToken));

    else if (update.Message.Text is not null)
        Task.Run(() => new MessageHandler().Handle(botClient, update, chats, cancellationToken));

    return;
}

static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
    CancellationToken cancellationToken) =>
    Console.WriteLine(exception);