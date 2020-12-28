using System;
using Common.ServiceBus;
using Common.Types;

namespace Common.IntegrationEvents
{
    public class OfferChangedIntegrationEvent : IntegrationEvent
    {
        public Guid OfferId { get; init; }
        public ChangeState<string> Name { get; init; }
        public ChangeState<string> Description { get; init; }
        public ChangeState<decimal?> Price { get; init; }
        public ChangeState<int?> AvailableStock { get; init; }
    }
}
