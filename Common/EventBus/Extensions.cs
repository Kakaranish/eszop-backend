using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Logging;
using RawRabbit.vNext;
using System;
using System.Linq;
using System.Reflection;

namespace Common.EventBus
{
    public static class Extensions
    {
        public static EventBusBuilder AddEventBus(this IServiceCollection services)
        {
            RegisterIntegrationEventHandlers(services);

            return UseAzureEventBus(services)
                ? services.AddAzureEventBus()
                : services.AddRabbitMqEventBus();
        }

        public static EventBusBuilder AddAzureEventBus(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            services.Configure<AzureEventBusConfig>(configuration.GetSection("EventBus:AzureEventBus"));

            services.AddSingleton<IEventBus, AzureEventBus>();

            serviceProvider = services.BuildServiceProvider();
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            return new EventBusBuilder(eventBus);
        }

        public static EventBusBuilder AddRabbitMqEventBus(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            services.AddSingleton<IServiceProvider>();

            var rabbitMqConfig = new RawRabbitConfiguration();
            rabbitMqConfig.Hostnames.Clear();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            configuration.GetSection("EventBus:RabbitMq").Bind(rabbitMqConfig);

            var busClient = BusClientFactory.CreateDefault(
                new ServiceCollection().AddRawRabbit(custom: ioc =>
                {
                    ioc.AddSingleton(_ => rabbitMqConfig);
                    ioc.AddSingleton<ILoggerFactory, RawRabbit.Logging.Serilog.LoggerFactory>();
                }));

            services.AddSingleton<IBusClient>(busClient);
            services.AddSingleton<IEventBus, RabbitMqEventBus>();

            serviceProvider = services.BuildServiceProvider();
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            return new EventBusBuilder(eventBus);
        }

        private static void RegisterIntegrationEventHandlers(IServiceCollection services)
        {
            var assembly = Assembly.GetEntryAssembly();
            var implementingTypes = assembly.GetTypes().Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                (t.BaseType?.IsGenericType ?? false) &&
                t.BaseType.IsAbstract &&
                t.BaseType.GetGenericTypeDefinition() == typeof(IntegrationEventHandler<>)
            );

            foreach (var implementingType in implementingTypes)
            {
                var integrationEventType = implementingType.BaseType.GetGenericArguments().First();
                var handlerType = typeof(IntegrationEventHandler<>).MakeGenericType(integrationEventType);
                services.AddScoped(handlerType, implementingType);
            }
        }

        private static bool UseAzureEventBus(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetValue<bool>("EventBus:UseAzureEventBus");
        }
    }
}
