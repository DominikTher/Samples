using DependencyInjection.Interfaces;
using System;

namespace DependencyInjection.Services
{
    class SmsNotificationService : INotificationService
    {
        public void NotifyToConsole()
        {
            Console.WriteLine("Sms notification");
        }
    }
}
