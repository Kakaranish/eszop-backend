using System;
using Common.Domain;
using Common.Types;

namespace Offers.API.Application.DomainEvents.AvailableStockChanged
{
    public class AvailableStockChangedDomainEvent : IDomainEvent
    {
        public Guid OfferId { get; init; }
        public ChangeState<int?> AvailableStock { get; init; }
    }
}
