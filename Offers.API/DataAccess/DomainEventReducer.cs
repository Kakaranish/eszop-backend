using Common.Domain;
using Common.EventBus;
using Offers.API.Domain;
using System.Collections.Generic;

namespace Offers.API.DataAccess
{
    public class DomainEventReducer : IEventReducer
    {
        public IEnumerable<IDomainEvent> Reduce<T>(T entity) where T : EntityBase
        {
            if (entity is Offer offer) return ReduceOfferEvents(offer);

            return entity.DomainEvents;
        }

        public IEnumerable<IDomainEvent> ReduceOfferEvents(Offer offer)
        {
            return offer.DomainEvents;
        }
    }
}
