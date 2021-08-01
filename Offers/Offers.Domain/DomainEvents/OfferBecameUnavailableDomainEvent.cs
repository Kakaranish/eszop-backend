using Common.Domain.DomainEvents;
using Common.Domain.Types;
using System;

namespace Offers.Domain.DomainEvents
{
    public class OfferBecameUnavailableDomainEvent : IDomainEvent
    {
        public Guid OfferId { get; init; }
        public UnavailabilityTrigger Trigger { get; init; }
    }
}
