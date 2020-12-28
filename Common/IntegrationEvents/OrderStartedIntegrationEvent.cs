using System;
using Common.EventBus;

namespace Common.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public Guid OrderId { get; init; }
    }
}
