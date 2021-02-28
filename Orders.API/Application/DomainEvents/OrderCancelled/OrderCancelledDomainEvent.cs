using Common.Domain;
using Common.Dto;
using Orders.API.Domain;
using System;
using System.Collections.Generic;

namespace Orders.API.Application.DomainEvents.OrderCancelled
{
    public class OrderCancelledDomainEvent : IDomainEvent
    {
        public Guid OrderId { get; init; }
        public OrderState PreviousState { get; init; }
        public OrderState CurrentState { get; init; }
        public IList<OrderItemDto> OrderItems { get; init; }
    }
}
