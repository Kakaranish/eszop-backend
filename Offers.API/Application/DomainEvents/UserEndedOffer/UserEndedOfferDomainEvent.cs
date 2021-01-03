using Common.Domain;
using System;

namespace Offers.API.Application.DomainEvents.UserEndedOffer
{
    public class UserEndedOfferDomainEvent : IDomainEvent
    {
        public Guid OfferId { get; init; }
    }
}
