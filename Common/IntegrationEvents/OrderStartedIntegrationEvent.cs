using System;
using Common.EventBus;

namespace Common.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid OfferId { get; set; }
        public int Quantity { get; set; }
    }
}
