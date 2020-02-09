using DependencyInjection.Configuration;
using DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = Bootstrap.GetConfiguration();
            using var serviceProvider = Bootstrap.GetServiceProvider(configuration);

            var consoleApplication = serviceProvider.GetService<IApplication>();
            consoleApplication.PrintDescriptionToConsole();
            consoleApplication.Run();
        }
    }
}
