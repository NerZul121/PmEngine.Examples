using PmEngine.Core.BaseClasses;
using PmEngine.Core.Interfaces;

namespace PmEngine.Examples
{
    /// <summary>
    /// Модуль для PmEngine
    /// </summary>
    public class ExampleModule : BaseModuleRegistrator
    {
    }

    /// <summary>
    /// Класс для регистрации модуля
    /// </summary>
    public static class Examples
    {
        /// <summary>
        /// Добавляем модуль в приложение
        /// </summary>
        /// <param name="services"></param>
        public static void AddExamples(this IServiceCollection services)
        {
            services.AddSingleton<IModuleRegistrator>(new ExampleModule());
        }
    }
}