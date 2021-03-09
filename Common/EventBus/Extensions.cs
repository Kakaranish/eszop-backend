using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
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

            return new RabbitMqEventBusBuilder(eventBus);
        }
    }
}
