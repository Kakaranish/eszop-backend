using RawRabbit;
using System;
using System.Threading.Tasks;

namespace Common.ServiceBus
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly IBusClient _busClient;

        public RabbitMqEventBus(IBusClient busClient)
        {
            _busClient = busClient ?? throw new ArgumentNullException(nameof(busClient));
        }

        public Task SubscribeAsync<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IntegrationEventHandler<TEvent>
        {
            _busClient.SubscribeAsync<TEvent>(async (@event, _) =>
            {
                var handler = Activator.CreateInstance<TEventHandler>();
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
