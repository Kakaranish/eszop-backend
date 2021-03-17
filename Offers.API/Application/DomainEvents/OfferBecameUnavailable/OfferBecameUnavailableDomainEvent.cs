using Common.Domain;
using Common.EventBus.IntegrationEvents;
using System;

namespace Offers.API.Application.DomainEvents.OfferBecameUnavailable
{
    public class OfferBecameUnavailableDomainEvent : IDomainEvent
    {
        public Guid OfferId { get; init; }
        public UnavailabilityTrigger Trigger { get; init; }
    }
}
