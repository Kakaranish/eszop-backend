using Common.Domain.DomainEvents;
using Orders.API.Domain;
using System;

namespace Orders.API.Application.DomainEvents.OrderStatusChanged
{
    public class OrderStatusChangedDomainEvent : IDomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid BuyerId { get; init; }
        public OrderState PreviousState { get; init; }
        public OrderState CurrentState { get; init; }
    }
}
