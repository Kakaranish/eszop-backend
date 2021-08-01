using System;

namespace Common.Utilities.EventBus
{
    public class EventBusBuilder
    {
        private readonly IEventBus _eventBus;

        public EventBusBuilder(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public EventBusBuilder Subscribe<TIntegrationEvent, TIntegrationEventHandler>()
            where TIntegrationEvent : IntegrationEvent
            where TIntegrationEventHandler : IntegrationEventHandler<TIntegrationEvent>
        {
            _eventBus.SubscribeAsync<TIntegrationEvent, TIntegrationEventHandler>();

            return this;
        }
    }
}
