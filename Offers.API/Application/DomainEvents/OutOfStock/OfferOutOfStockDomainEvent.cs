using Common.Types.Domain;
using System;

namespace Offers.API.Application.DomainEvents.OutOfStock
{
    public class OfferOutOfStockDomainEvent : IDomainEvent
    {
        public Guid OfferId { get; init; }
    }
}
