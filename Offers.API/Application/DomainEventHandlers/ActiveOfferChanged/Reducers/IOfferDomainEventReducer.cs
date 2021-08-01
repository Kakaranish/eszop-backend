using System.Collections.Generic;
using Common.Domain.DomainEvents;
using Offers.Domain;
using Offers.Domain.Aggregates.OfferAggregate;

namespace Offers.API.Application.DomainEventHandlers.ActiveOfferChanged.Reducers
{
    public interface IOfferDomainEventReducer
    {
        IEnumerable<IDomainEvent> Reduce(Offer offer);
    }
}
