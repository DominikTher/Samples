using DependencyInjection.Interfaces;
using System;

namespace DependencyInjection.Services
{
    class ClassicService : IService
    {
        private readonly Guid guid;

        public ClassicService()
        {
            guid = Guid.NewGuid();
        }

        public void PrintServiceNameToConsole()
        {
            Console.WriteLine($"{nameof(ClassicService)} - {guid}");
        }
    }
}
