using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;

namespace Common.EventBus
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly IBusClient _busClient;
        private IServiceProvider _serviceProvider;

        public RabbitMqEventBus(IBusClient busClient)
        {
            _busClient = busClient ?? throw new ArgumentNullException(nameof(busClient));
        }
        
        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
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
