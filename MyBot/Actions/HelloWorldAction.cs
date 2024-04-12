using PmEngine.Core;
using PmEngine.Core.BaseMarkups;
using PmEngine.Core.Interfaces;

namespace MyBot.Actions
{
    /// <summary>
    /// Пример простого действия пользователя.
    /// </summary>
    public class HelloWorldAction : IAction
    {
        public async Task<INextActionsMarkup?> DoAction(ActionWrapper currentAction, IUserSession user)
        {
            // Добавляем текст для пользователя
            user.AddToOutput($"Привет, {user.CachedData.Id}!");

            // Формируем список кнопок, которые он получит
            var result = new LinedMarkup();

            // Добавляем кнопку "Привет!", которая выполнит действие HelloWorldAction
            result.Add("Привет!", typeof(HelloWorldAction));

            // Возвращаем список кнопок
            return result;
        }
    }
}