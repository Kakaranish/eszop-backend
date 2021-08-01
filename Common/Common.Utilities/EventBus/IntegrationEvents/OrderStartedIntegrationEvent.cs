using Common.Dto;
using System;
using System.Collections.Generic;

namespace Common.Utilities.EventBus.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; init; }
        public IList<OrderItemDto> OrderItems { get; init; }
    }
}
