using Common.Extensions;
using Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;
using System.Linq;
using System.Reflection;
using ILoggerFactory = RawRabbit.Logging.ILoggerFactory;

namespace Common.EventBus
{
    public static class StartupExtensions
    {
        public static void AddEventBus(this IServiceCollection services)
        {
            services.RegisterIntegrationEventHandlers(Assembly.GetEntryAssembly());

            if (UseAzureEventBus(services)) services.AddAzureEventBus();
            else services.AddRabbitMqEventBus();
        }

        public static IServiceCollection AddAzureEventBus(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            const string connStrEnvVarName = "ESZOP_AZURE_EVENTBUS_CONN_STR";
            const string topicNameVarName = "ESZOP_AZURE_EVENTBUS_TOPIC_NAME";
            const string subNameVarName = "ESZOP_AZURE_EVENTBUS_SUB_NAME";

            var connStr = EnvironmentHelpers.GetRequiredEnvVariable(connStrEnvVarName);
            var topicName = configuration.GetRequiredConfigValue(topicNameVarName, "EventBus:AzureEventBus:TopicName");
            var subName = configuration.GetRequiredConfigValue(subNameVarName, "EventBus:AzureEventBus:SubscriptionName");

            services.Configure<AzureEventBusConfig>(config =>
            {
                config.ConnectionString = connStr;
                config.TopicName = topicName;
                config.SubscriptionName = subName;
            });

            services.AddSingleton<IEventBus, AzureEventBus>();
            return services;
        }

        public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

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

            return services;
        }

        public static IServiceCollection RegisterIntegrationEventHandlers(this IServiceCollection services, Assembly assembly)
        {
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

            return services;
        }

        private static bool UseAzureEventBus(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetValue<bool>("EventBus:UseAzureEventBus");
        }
    }
}
