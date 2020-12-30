using System;

namespace Common.EventBus
{
    public class RabbitMqEventBusBuilder
    {
        private readonly IEventBus _eventBus;

        public RabbitMqEventBusBuilder(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public RabbitMqEventBusBuilder Subscribe<TIntegrationEvent, TIntegrationEventHandler>()
            where TIntegrationEvent : IntegrationEvent
            where TIntegrationEventHandler : IntegrationEventHandler<TIntegrationEvent>
        {
            _eventBus.SubscribeAsync<TIntegrationEvent, TIntegrationEventHandler>();

            return this;
        }
    }
}
