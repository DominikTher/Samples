using DependencyInjection.Interfaces;
using System;

namespace DependencyInjection.Services
{
    public class SmsNotificationService : INotificationService
    {
        public void NotifyToConsole()
        {
            Console.WriteLine("Sms notification");
        }
    }
}
