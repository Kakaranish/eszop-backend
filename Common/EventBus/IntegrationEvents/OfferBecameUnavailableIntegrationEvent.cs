using System;
using Common.Domain.Types;

namespace Common.Utilities.EventBus.IntegrationEvents
{
    public class OfferBecameUnavailableIntegrationEvent : IntegrationEvent
    {
        public Guid OfferId { get; init; }
        public UnavailabilityTrigger Trigger { get; init; }
    }
}
