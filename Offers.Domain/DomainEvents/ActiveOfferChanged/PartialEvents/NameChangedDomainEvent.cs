using Common.Domain.DomainEvents;
using Common.Domain.Types;

namespace Offers.Domain.DomainEvents.ActiveOfferChanged.PartialEvents
{
    public class NameChangedDomainEvent : IPartialDomainEvent
    {
        public ChangeState<string> NameChange { get; init; }
    }
}
