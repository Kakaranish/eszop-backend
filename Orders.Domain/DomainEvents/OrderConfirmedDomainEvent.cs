using Common.Domain.DomainEvents;
using System;

namespace Orders.Domain.DomainEvents
{
    public class OrderConfirmedDomainEvent : IDomainEvent
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; init; }
        public Guid SellerId { get; init; }
    }
}
