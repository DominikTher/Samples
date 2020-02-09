using DependencyInjection.Configuration;
using DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DependencyInjection
{
    class ConsoleApplication : IApplication
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ConsoleApplicationOptions consoleApplicationOptions;

        public ConsoleApplication(IServiceProvider serviceProvider, ConsoleApplicationOptions consoleApplicationOptions)
        {
            this.serviceProvider = serviceProvider;
            this.consoleApplicationOptions = consoleApplicationOptions;
        }

        public void PrintDescriptionToConsole()
        {
            Console.WriteLine($"{consoleApplicationOptions.Description}{Environment.NewLine}");
        }

        public void Run()
        {
            serviceProvider.GetRequiredService<IConcreteService>().PrintServiceNameToConsole();
            serviceProvider.GetRequiredService<IConcreteService>().PrintNamesOfServicesToConsole();
            serviceProvider.GetRequiredService<INotificationService>().NotifyToConsole();
        }
    }
}
