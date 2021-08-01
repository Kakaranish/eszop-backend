using Common.Domain.DomainEvents;
using Orders.Domain.Aggregates.OrderAggregate;
using System;

namespace Orders.Domain.DomainEvents
{
    public class OrderStatusChangedDomainEvent : IDomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid BuyerId { get; init; }
        public OrderState PreviousState { get; init; }
        public OrderState CurrentState { get; init; }
    }
}
