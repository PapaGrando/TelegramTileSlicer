using Telegram.Bot;
using TelegramCropper;
using TelegramCropper.Factory;
using TelegramCropper.Interfaces;
using TelegramCropper.Repo;
using TelegramCropper.Services;

//Этот бот был перенесен с LongPolling (v 0.0.1) на Webhooks 
//Старую архитектуру на Commands решено было не трогать

//TODO: Пофиксить бардак с DI
//TODO: Вынести текст комманд в какой нить конфиг
//TODO: Добавить еще параметров в Config 
//TODO: Добавить фильтров и параметры к ним

var builder = WebApplication.CreateBuilder(args);

var botConfig = builder.Configuration.GetSection("Config").Get<Config>();
botConfig.CheckConfig();

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddHttpClient("TelegramBot")
    .AddTypedClient<ITelegramBotClient>((httpClient) => 
    {
        var bot = new TelegramBotClient(botConfig.ApiKey, httpClient);
        bot.ConfigureCommands();

        return bot;
    });

builder.Services.AddScoped<HandleUpdateService>();

builder.Services.AddSingleton<IChatRepo<IChatJob>>(x => 
    new ChatJobRepo(botConfig.MaxTasksLifeTimeSec, botConfig.MaxTaskProcessTimeoutSec));
builder.Services.AddSingleton<ICommandsFactory, CommandsFactory>();

builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();