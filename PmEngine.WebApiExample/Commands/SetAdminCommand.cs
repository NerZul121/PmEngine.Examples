using PmEngine.Core.Extensions;
using PmEngine.Core.Interfaces;

namespace PmEngine.WebApiExample.Commands
{
    /// <summary>
    /// Выдаем права админа для пользователя.
    /// </summary>
    public class SetAdminCommand : ICommand
    {
        public string Name => "setadmin2024";

        public string CommandPattern => "";

        public string Description => "";

        public int UserType => 0;

        public async Task<bool> DoCommand(string text, IUserSession user)
        {
            await user.Services.InContext(async (context) =>
            {
                // Перезагружаем данные пользователя из сессии в текущем контексте, для изменения закешированных данных в сессии и в бд одновременно.
                var data = await user.Reload(context);
                data.UserType = 2;
                await context.SaveChangesAsync();
            });

            await user.Output.ShowContent("Теперь вы админ!");

            return true;
        }
    }
}