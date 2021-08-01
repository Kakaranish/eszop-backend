using Common.Domain.DomainEvents;
using Offers.Domain.Aggregates.OfferAggregate;
using System.Collections.Generic;

namespace Offers.API.Application.DomainEventHandlers.ActiveOfferChanged.Reducers
{
    public interface IOfferDomainEventReducer
    {
        IEnumerable<IDomainEvent> Reduce(Offer offer);
    }
}
