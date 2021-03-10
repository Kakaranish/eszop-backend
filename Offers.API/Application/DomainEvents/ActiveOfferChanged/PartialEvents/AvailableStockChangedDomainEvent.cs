using Common.Domain;
using Common.Types;

namespace Offers.API.Application.DomainEvents.ActiveOfferChanged.PartialEvents
{
    public class AvailableStockChangedDomainEvent : IPartialDomainEvent
    {
        public ChangeState<int> AvailableStockChange { get; init; }
    }
}
