using PmEngine.Core.BaseMarkups;
using PmEngine.Core.Interfaces;

namespace PmEngine.Examples.Actions
{
    public class HelloWorldAction : IAction
    {
        public async Task<INextActionsMarkup?> DoAction(IActionWrapper currentAction, IUserSession user, IActionArguments arguments)
        {
            user.OutputContent += $"Привет, {user.CachedData.Name}!";

            var result = new SingleMarkup();

            result.Add("Привет!", typeof(HelloWorldAction));

            return result;
        }
    }
}