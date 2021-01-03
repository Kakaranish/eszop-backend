using System;

namespace Common.EventBus.IntegrationEvents
{
    public class UserEndedOfferIntegrationEvent : IntegrationEvent
    {
        public Guid OfferId { get; init; }
    }
}
