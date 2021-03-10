using Common.Domain;
using Common.Types;

namespace Offers.API.Application.DomainEvents.ActiveOfferChanged.PartialEvents
{
    public class MainImageChangedDomainEvent : IPartialDomainEvent
    {
        public ChangeState<string> MainImageUriChange { get; init; }
    }
}
