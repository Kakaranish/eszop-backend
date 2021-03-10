using Common.Domain;
using Common.Types;

namespace Offers.API.Application.DomainEvents.ActiveOfferChanged.PartialEvents
{
    public class PriceChangedDomainEvent : IPartialDomainEvent
    {
        public ChangeState<decimal> PriceChange { get; init; }
    }
}
