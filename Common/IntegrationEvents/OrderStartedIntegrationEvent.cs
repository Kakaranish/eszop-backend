using System;
using Common.EventBus;

namespace Common.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid OfferId { get; init; }
        public int Quantity { get; init; }
    }
}
