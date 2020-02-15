using DependencyInjection.Interfaces;
using System;

namespace DependencyInjection.Services
{
    public class EmailNotificationService : INotificationService
    {
        public void NotifyToConsole()
        {
            Console.WriteLine("Email notification");
        }
    }
}
