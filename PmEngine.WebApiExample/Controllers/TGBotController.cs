using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using PmEngine.Telegram;

namespace PmEngine.WebApiExample.Controllers
{
    public class TGBotController : ControllerBase
    {
        private readonly ILogger<TGBotController> _logger;
        private readonly ITelegramBotClient _client;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Контолер
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="botClient"></param>
        public TGBotController(IServiceProvider services, ILogger<TGBotController> logger, ITelegramBotClient botClient)
        {
            _logger = logger;
            _client = botClient;
            _serviceProvider = services;
        }

        /// <summary>
        /// Прием сообщения из тг
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Update update)
        {
            var tgcontroller = new BaseTGController();
            await tgcontroller.Post(update, _client, _logger, _serviceProvider);
        }
    }
}