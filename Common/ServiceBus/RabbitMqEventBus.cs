using RawRabbit;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Common.ServiceBus
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqEventBus(IBusClient busClient, IServiceProvider serviceProvider)
        {
            _busClient = busClient ?? throw new ArgumentNullException(nameof(busClient));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Task SubscribeAsync<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IntegrationEventHandler<TEvent>
        {
            _busClient.SubscribeAsync<TEvent>(async (@event, _) =>
            {
                var handler = ActivatorUtilities.CreateInstance<TEventHandler>(_serviceProvider);
                await handler.Handle(@event);
            });

            return Task.CompletedTask;
        }
        
        public async Task PublishAsync<TEvent>(TEvent integrationEvent)
            where TEvent : IntegrationEvent
        {
            await _busClient.PublishAsync(integrationEvent);
        }
    }
}
