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

// ����������� �����������
using ILoggerFactory loggerFactory = LoggerFactory.Create((a) => { a.AddConsole(); });
builder.Services.AddSingleton(loggerFactory);
builder.Services.AddSingleton<ILogger>(loggerFactory.CreateLogger(""));
builder.Services.AddLogging((lf) => lf.AddConsole());

// ��������� ��� ������
builder.Services.AddExamples();
// ��������� ������ � �������
builder.Services.AddTelegramModule(tg => tg.DefaultInLineMessageAction = PmEngine.Telegram.Enums.MessageActionType.Delete);
// ��������� ������ � ��
builder.Services.AddVkModule();
// ��������� ������
builder.Services.AddPMEngine((e) =>
{
    // ������ ����� ������������ HelloWorldAction ��� ������� �������� ������������ ��� �������������
    e.Properties.InitializationAction = typeof(HelloWorldAction);
    // ������ ����� �������� �� � ������
    e.Properties.DataProvider = PmEngine.Core.Enums.DataProvider.InMemory;
});

// �������� �����
builder.Services.AddCors();
// ������� ���-��� ��� ������
builder.Services.AddHttpClient("tgwebhook").AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(envBotToken, httpClient));

// �������� ����������� � NewtonJson
builder.Services.AddControllers();

var app = builder.Build();

// ������������� ������
app.ConfigureEngine();

app.UseDeveloperExceptionPage();

app.UseForwardedHeaders();
app.UseRouting();
app.UseCors(cr => cr.AllowAnyOrigin());

// ������������� ����������� ���������� ���-����.
await app.SetDefaultTgWebhook();

app.Run();