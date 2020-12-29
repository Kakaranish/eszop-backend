using System;
using Common.Types;
using Common.Types.Domain;

namespace Offers.API.Application.DomainEvents.AvailableStockChanged
{
    public class AvailableStockChangedDomainEvent : IDomainEvent
    {
        public Guid OfferId { get; init; }
        public ChangeState<int?> AvailableStock { get; init; }
    }
}
