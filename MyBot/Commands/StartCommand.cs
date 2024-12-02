using MyBot.Actions;
using PmEngine.Core;
using PmEngine.Core.Interfaces;

namespace PmEngine.WebApiExample.Commands
{
    /// <summary>
    /// Пример команды /start
    /// </summary>
    public class StartCommand : ICommand
    {
        /// <summary>
        /// Регистрируем команду с именем start
        /// </summary>
        public string Name => "start";

        public string CommandPattern => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        /// <summary>
        /// Указываем, что ей могут пользоваться все
        /// </summary>
        public int UserType => 0;

        /// <summary>
        /// Реализуем код команды
        /// </summary>
        /// <param name="text"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> DoCommand(string text, IUserSession user)
        {
            // Если сессия пользователя была создана давно, то выполняем helloWorldAction
            if ((DateTime.Now - user.SessionCreateTime).TotalSeconds > 5)
                await user.ActionProcess(new ActionWrapper<HelloWorldAction>("start"));

            return true;
        }
    }
}