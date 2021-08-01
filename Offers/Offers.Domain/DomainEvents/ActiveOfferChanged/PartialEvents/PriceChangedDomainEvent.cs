using Common.Domain.DomainEvents;
using Common.Domain.Types;

namespace Offers.Domain.DomainEvents.ActiveOfferChanged.PartialEvents
{
    public class PriceChangedDomainEvent : IPartialDomainEvent
    {
        public ChangeState<decimal> PriceChange { get; init; }
    }
}
