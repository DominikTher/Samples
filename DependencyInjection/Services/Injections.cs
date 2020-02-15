using DependencyInjection.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.Services
{
    public class Injections
    {
        private readonly IService service;
        private readonly SmsNotificationService smsNotificationService;
        private readonly EmailNotificationService emailNotificationService;

        public Injections(IService service)
        {
            this.service = service;
        }

        public Injections(IService service, SmsNotificationService smsNotificationService)
        {
            this.smsNotificationService = smsNotificationService;
        }

        // Wrong
        //public Injections(IService service, EmailNotificationService emailNotificationService)
        //{
        //    this.emailNotificationService = emailNotificationService;
        //}

        // Action Injection, for Action in MVC or API maybe
        //public void Index([FromServices] EmailNotificationService emailNotificationService)
        //{
        //    emailNotificationService.NotifyToConsole();
        //}
    }
}
