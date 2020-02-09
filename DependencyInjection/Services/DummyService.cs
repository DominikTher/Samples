using DependencyInjection.Interfaces;
using System;

namespace DependencyInjection.Services
{
    class DummyService : IService
    {
        private readonly Guid guid;

        public DummyService()
        {
            guid = Guid.NewGuid();
        }

        public void PrintServiceNameToConsole()
        {
            Console.WriteLine($"{nameof(DummyService)} - {guid}");
        }
    }
}
