using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.vNext;

namespace Common.ServiceBus
{
    public static class Extensions
    {
        public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services)
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var rabbitMqConfig = new RawRabbitConfiguration();
            configuration.GetSection("EventBus:RabbitMq").Bind(rabbitMqConfig);
            var busClient = BusClientFactory.CreateDefault(rabbitMqConfig);
            var eventBus = new RabbitMqEventBus(busClient);
            
            services.AddSingleton<IEventBus>(eventBus);

            return services;
        }
    }
}
