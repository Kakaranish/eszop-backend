using System;
using Common.ServiceBus;
using Common.Types;

namespace Common.IntegrationEvents
{
    public class OfferChangedEvent : IntegrationEvent
    {
        public Guid OfferId { get;}

        public ChangeState<string> Name { get; init; }
        public ChangeState<string> Description { get; init; }
        public ChangeState<decimal?> Price { get; init; }

        public OfferChangedEvent(Guid offerId)
        {
            OfferId = offerId;
        }
    }
}
