using DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;

namespace DependencyInjection.Services
{
    class ConcreteService : IConcreteService
    {
        private readonly IEnumerable<IService> services;

        // IEnumerable<IService> services contain all registered services with this interface
        public ConcreteService(IEnumerable<IService> services)
        {
            this.services = services;
        }

        public void PrintNamesOfServicesToConsole()
        {
            foreach (var service in services)
            {
                service.PrintServiceNameToConsole();
            }

        }

        public void PrintServiceNameToConsole()
        {
            Console.WriteLine($"{nameof(ConcreteService)} - {Guid.NewGuid()}");
        }
    }
}
