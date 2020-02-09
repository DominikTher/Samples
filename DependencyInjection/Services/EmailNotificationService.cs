using DependencyInjection.Interfaces;
using System;

namespace DependencyInjection.Services
{
    class EmailNotificationService : INotificationService
    {
        public void NotifyToConsole()
        {
            Console.WriteLine("Email notification");
        }
    }
}
