using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using PmEngine.Core.Daemons;
using PmEngine.Core.Extensions;
using PmEngine.Examples;
using PmEngine.Examples.Actions;
using PmEngine.Telegram;
using PmEngine.Vk;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
var envBotToken = Environment.GetEnvironmentVariable("BOT_TOKEN") ?? "";

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// Настраиваем логирование
using ILoggerFactory loggerFactory = LoggerFactory.Create((a) => { a.AddConsole(); });
builder.Services.AddSingleton(loggerFactory);
builder.Services.AddSingleton<ILogger>(loggerFactory.CreateLogger(""));
builder.Services.AddLogging((lf) => lf.AddConsole());

// Добавляем наш модуль
builder.Services.AddExamples();
// Добавляем работу с телегой
builder.Services.AddTelegramModule(tg => tg.DefaultInLineMessageAction = PmEngine.Telegram.Enums.MessageActionType.Delete);
// Добавляем работу с ВК
builder.Services.AddVkModule();
// Добавляем движок
builder.Services.AddPMEngine((e) =>
{
    // Движок будет использовать HelloWorldAction как базовое действие пользователя при инициализации
    e.Properties.InitializationAction = typeof(HelloWorldAction);
    // Движок будет работать БД в памяти
    e.Properties.DataProvider = PmEngine.Core.Enums.DataProvider.InMemory;
});

//Добавляем для теста демона в качестве мягкой ссылки. Он будет работать только тогда, когда либа будет находиться в папке, откуда запускается бот.
DaemonManager.DaemonsToLoad.Add("MyBot.TestDaemon");

// Включаем корсы
builder.Services.AddCors();
// Создаем веб-хук для телеги
builder.Services.AddHttpClient("tgwebhook").AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(envBotToken, httpClient));

// Включаем контроллеры с NewtonJson
builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
});

var app = builder.Build();

// Конфигурируем движок
app.ConfigureEngine();

app.UseDeveloperExceptionPage();

app.UseForwardedHeaders();
app.UseRouting();
app.UseCors(cr => cr.AllowAnyOrigin());

app.UseStaticFiles();

// Включаем контроллер для веб-хука телеги
app.UseEndpoints(ep =>
{
    ep.MapControllerRoute(name: "tgwebhook",
        pattern: $"TGBot/{envBotToken}",
        new { controller = "TGBot", action = "Post" });
    ep.MapControllers();
});

app.Run();