using Common.Domain;
using Common.Domain.EventDispatching;
using Offers.API.Domain;
using System;
using System.Collections.Generic;

namespace Offers.API.Application.DomainEvents.Reducers
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
