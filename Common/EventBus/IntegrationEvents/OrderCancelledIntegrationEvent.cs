using Common.Dto;
using System;
using System.Collections.Generic;

namespace Common.EventBus.IntegrationEvents
{
    public class OrderCancelledIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; init; }
        public OrderStateDto PreviousState { get; init; }
        public OrderStateDto CurrentState { get; init; }
        public IList<OrderItemDto> OrderItems { get; init; }
    }
}
