using DependencyInjection.Interfaces;
using System;

namespace DependencyInjection.Services
{
    class FakeService : IService
    {
        private readonly Guid guid;

        public FakeService()
        {
            guid = Guid.NewGuid();
        }
        public void PrintServiceNameToConsole()
        {
            Console.WriteLine($"{nameof(FakeService)} - {guid}");
        }
    }
}
