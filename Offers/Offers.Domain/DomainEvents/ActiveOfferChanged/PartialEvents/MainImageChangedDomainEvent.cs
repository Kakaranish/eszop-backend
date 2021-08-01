using Common.Domain.DomainEvents;
using Common.Domain.Types;

namespace Offers.Domain.DomainEvents.ActiveOfferChanged.PartialEvents
{
    public class MainImageChangedDomainEvent : IPartialDomainEvent
    {
        public ChangeState<string> MainImageUriChange { get; init; }
    }
}
