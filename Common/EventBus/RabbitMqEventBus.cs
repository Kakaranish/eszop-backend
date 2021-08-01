using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;

namespace Common.Utilities.EventBus
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _provider;

        public RabbitMqEventBus(IBusClient busClient, IServiceProvider provider)
        {
            _busClient = busClient ?? throw new ArgumentNullException(nameof(busClient));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public Task SubscribeAsync<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IntegrationEventHandler<TEvent>
        {
            _busClient.SubscribeAsync<TEvent>(async (@event, _) =>
            {
                using var scope = _provider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IntegrationEventHandler<TEvent>>();

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
