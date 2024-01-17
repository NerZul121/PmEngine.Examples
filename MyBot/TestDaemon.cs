using Microsoft.Extensions.Logging;
using PmEngine.Core;
using PmEngine.Core.Daemons;

namespace MyBot
{
    public class TestDaemon : BaseDaemon
    {
        public TestDaemon(IServiceProvider services, ILogger logger) : base(services, logger)
        {
            DelayInSec = 10;
        }

        /// <summary>
        /// Тут должна лежать логика демона
        /// </summary>
        /// <returns></returns>
        /// Попробуй изменить код, сбилдить, подложить новую либу в каталог работоющего бота, выдать себе права админа (/setadmin2024) и выполнить команду /daemonreload MyBot.TestDaemon
        public override async Task Work()
        {
            _logger.LogInformation("I'm worked");
        }
    }
}