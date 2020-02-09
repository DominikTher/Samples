using DependencyInjection.Interfaces;
using DependencyInjection.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.IO;

namespace DependencyInjection.Configuration
{
    static class Bootstrap
    {
        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static ServiceProvider GetServiceProvider(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IApplication, ConsoleApplication>();

            // Example of service descriptor. There is more methods how to create service descriptor
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicedescriptor?view=dotnet-plat-ext-3.1
            var concreteServiceDescriptor = new ServiceDescriptor(typeof(IConcreteService), typeof(ConcreteService), ServiceLifetime.Transient);
            serviceCollection.Add(concreteServiceDescriptor);

            serviceCollection.Configure<ConsoleApplicationOptions>(configuration.GetSection("ConsoleApplicationOptions"));
            serviceCollection.AddSingleton(service => service.GetRequiredService<IOptions<ConsoleApplicationOptions>>().Value); // Simplify registration of IOptions<>

            // Example: Singleton as parameter for registration against multiple interfaces, we need use same object.
            // But also possible register two independet singletons, it depends on situation. But be aware of memory usage
            // serviceCollection.AddSingleton<SomeService>();
            // serviceCollection.AddSingleton<IFirstService>(sp => sp.GetRequiredService<SomeService>());
            // serviceCollection.AddSingleton<ISecondService>(sp => sp.GetRequiredService<SomeService>());

            // Example: Generic registration, also possible with custom types
            //services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            // Extension method
            serviceCollection
                .AddEnumerableServicesExample()
                .AddCompositeExample();

            // Avoid accidentally replaced the previous registration
            //serviceCollection.TryAddTransient<IService, FakeService>();

            // Replace service, only support removing first registration
            //serviceCollection.Replace(ServiceDescriptor.Singleton<IService, FakeService>());

            return serviceCollection.BuildServiceProvider();
        }

        // Put it to the namespace: Microsoft.Extensions.DependencyInjection
        public static IServiceCollection AddEnumerableServicesExample(this IServiceCollection serviceCollection)
        {
            // Example: register multiple interfaces when DI parameter is IEnumerable<>

            // Case 1: one by one, possible mistakes, forgot to register some service
            //serviceCollection.AddTransient<IService, DummyService>();
            //serviceCollection.AddTransient<IService, ClassicService>();
            //serviceCollection.AddTransient<IService, ClassicService>(); // Duplication! Also execute, might be problem in some cases!

            // Case 2: Assembly scanning - not possible to do in .net core DI

            // Case 3: Avoid duplicates
            serviceCollection.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Transient<IService, DummyService>(),
                ServiceDescriptor.Transient<IService, FakeService>(),
                ServiceDescriptor.Transient<IService, ClassicService>(),
                ServiceDescriptor.Transient<IService, ClassicService>() // Only one is added and executed
            });

            return serviceCollection;
        }

        // Put it to the namespace: Microsoft.Extensions.DependencyInjection
        public static IServiceCollection AddCompositeExample(this IServiceCollection serviceCollection)
        {
            // Example: Composite
            serviceCollection.AddSingleton<SmsNotificationService>();
            serviceCollection.AddSingleton<EmailNotificationService>();

            serviceCollection.AddSingleton<INotificationService>(service =>
                new CompositeNotificationService(
                    new INotificationService[] {
                        service.GetRequiredService<EmailNotificationService>(),
                        service.GetRequiredService<SmsNotificationService>()
                    }
                ));

            return serviceCollection;
        }
    }
}
