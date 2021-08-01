using Common.Dto;
using System;
using System.Collections.Generic;

namespace Common.Utilities.EventBus.IntegrationEvents
{
    public class OrderCancelledIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; init; }
        public string PreviousState { get; init; }
        public string CurrentState { get; init; }
        public IList<OrderItemDto> OrderItems { get; init; }
    }
}
