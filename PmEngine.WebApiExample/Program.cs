using Microsoft.AspNetCore.HttpOverrides;
using MyBot.Actions;
using PmEngine.Core.Extensions;
using PmEngine.Examples;
using PmEngine.Telegram;
using PmEngine.Vk;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
var envBotToken = Environment.GetEnvironmentVariable("BOT_TOKEN") ?? "";
//Environment.SetEnvironmentVariable("HOST_URL", "https://c678-185-197-33-14.ngrok-free.app");

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

// Включаем корсы
builder.Services.AddCors();
// Создаем веб-хук для телеги
builder.Services.AddHttpClient("tgwebhook").AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(envBotToken, httpClient));

// Включаем контроллеры с NewtonJson
builder.Services.AddControllers();

var app = builder.Build();

// Конфигурируем движок
app.ConfigureEngine();

app.UseDeveloperExceptionPage();

app.UseForwardedHeaders();
app.UseRouting();
app.UseCors(cr => cr.AllowAnyOrigin());

// Устанавливаем стандартную реализацию веб-хука.
await app.SetDefaultTgWebhook();

app.Run();