using DependencyInjection.Interfaces;
using System.Collections.Generic;

namespace DependencyInjection.Services
{
    class CompositeNotificationService : INotificationService
    {
        private readonly IEnumerable<INotificationService> notificationServices;

        public CompositeNotificationService(IEnumerable<INotificationService> notificationServices)
        {
            this.notificationServices = notificationServices;
        }

        public void NotifyToConsole()
        {
            foreach (var notificationService in notificationServices)
            {
                notificationService.NotifyToConsole();
            }
        }
    }
}
