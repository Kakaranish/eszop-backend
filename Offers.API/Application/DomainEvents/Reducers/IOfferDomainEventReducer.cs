using Common.Domain;
using Offers.API.Domain;
using System.Collections.Generic;

namespace Offers.API.Application.DomainEvents.Reducers
{
    public interface IOfferDomainEventReducer
    {
        IEnumerable<IDomainEvent> Reduce(Offer offer);
    }
}
