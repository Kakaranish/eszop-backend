using Common.Domain.DomainEvents;
using Common.Dto;
using Orders.Domain.Aggregates.OrderAggregate;
using System;
using System.Collections.Generic;

namespace Orders.Domain.DomainEvents
{
    public class OrderCancelledDomainEvent : IDomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid BuyerId { get; init; }
        public Guid SellerId { get; init; }
        public OrderState PreviousState { get; init; }
        public OrderState CurrentState { get; init; }
        public IList<OrderItemDto> OrderItems { get; init; }
    }
}
