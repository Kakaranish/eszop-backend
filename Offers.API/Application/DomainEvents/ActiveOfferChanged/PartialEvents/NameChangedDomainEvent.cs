using Common.Domain;
using Common.Types;

namespace Offers.API.Application.DomainEvents.ActiveOfferChanged.PartialEvents
{
    public class NameChangedDomainEvent : IPartialDomainEvent
    {
        public ChangeState<string> NameChange { get; init; }
    }
}
