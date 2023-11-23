using Microsoft.Extensions.DependencyInjection;
using PmEngine.Core.BaseClasses;
using PmEngine.Core.Interfaces;

namespace PmEngine.Examples
{
    public class ExampleModule : BaseModuleRegistrator
    {
    }

    public static class Examples
    {
        public static void AddExamples(this IServiceCollection services)
        {
            services.AddSingleton<IModuleRegistrator>(new ExampleModule());
        }
    }
}