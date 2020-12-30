using System;

namespace Common.EventBus.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid OfferId { get; init; }
        public int Quantity { get; init; }
    }
}
