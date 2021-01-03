using System;
using Common.Domain;

namespace Offers.API.Application.DomainEvents.OfferBecameUnavailable
{
    public class OfferBecameUnavailableDomainEvent : IDomainEvent
    {
        public Guid OfferId { get; init; }
    }
}
