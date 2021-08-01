using Common.Domain;
using Common.Domain.DomainEvents;
using Common.Domain.EventDispatching;
using Offers.Domain.Aggregates.OfferAggregate;
using System;
using System.Collections.Generic;

namespace Offers.API.Application.DomainEventHandlers.ActiveOfferChanged.Reducers
{
    public class DomainEventReducer : IEventReducer
    {
        private readonly IOfferDomainEventReducer _offerDomainEventReducer;

        public DomainEventReducer(IOfferDomainEventReducer offerDomainEventReducer)
        {
            _offerDomainEventReducer = offerDomainEventReducer ?? throw new ArgumentNullException(nameof(offerDomainEventReducer));
        }

        public IEnumerable<IDomainEvent> Reduce<T>(T entity) where T : EntityBase
        {
            if (entity is Offer offer) return _offerDomainEventReducer.Reduce(offer);

            return entity.DomainEvents;
        }
    }
}
