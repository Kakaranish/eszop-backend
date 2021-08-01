using Common.Domain.DomainEvents;
using System;

namespace Orders.API.Application.DomainEvents.OrderConfirmed
{
    public class OrderConfirmedDomainEvent : IDomainEvent
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; init; }
        public Guid SellerId { get; init; }
    }
}
