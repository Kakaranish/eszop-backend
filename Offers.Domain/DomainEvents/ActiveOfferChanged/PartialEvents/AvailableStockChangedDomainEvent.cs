using Common.Domain.DomainEvents;
using Common.Domain.Types;

namespace Offers.Domain.DomainEvents.ActiveOfferChanged.PartialEvents
{
    public class AvailableStockChangedDomainEvent : IPartialDomainEvent
    {
        public ChangeState<int> AvailableStockChange { get; init; }
    }
}
