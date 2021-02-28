using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.Logging;
using RawRabbit.vNext;

namespace Common.EventBus
{
    public static class Extensions
    {
        public static RabbitMqEventBusBuilder AddRabbitMqEventBus(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var rabbitMqConfig = new RawRabbitConfiguration();
            rabbitMqConfig.Hostnames.Clear();
            configuration.GetSection("EventBus:RabbitMq").Bind(rabbitMqConfig);
            
            var busClient = BusClientFactory.CreateDefault(
                new ServiceCollection().AddRawRabbit(custom: ioc =>
                {
                    ioc.AddSingleton(_ => rabbitMqConfig);
                    ioc.AddSingleton<ILoggerFactory, RawRabbit.Logging.Serilog.LoggerFactory>();
                }));

            var eventBus = new RabbitMqEventBus(busClient);
            services.AddSingleton<IEventBus>(eventBus);
            serviceProvider = services.BuildServiceProvider();
            eventBus.SetServiceProvider(serviceProvider);

            return new RabbitMqEventBusBuilder(eventBus);
        }
    }
}
