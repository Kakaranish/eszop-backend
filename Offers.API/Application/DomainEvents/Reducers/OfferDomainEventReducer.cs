using Common.Domain;
using Offers.API.Application.DomainEvents.ActiveOfferChanged;
using Offers.API.Application.DomainEvents.ActiveOfferChanged.PartialEvents;
using Offers.API.Domain;
using System.Collections.Generic;

namespace Offers.API.Application.DomainEvents.Reducers
{
    public class OfferDomainEventReducer : IOfferDomainEventReducer
    {
        public IEnumerable<IDomainEvent> Reduce(Offer offer)
        {
            var domainEvents = new List<IDomainEvent>();
            ActiveOfferChangedDomainEvent activeOfferChanged = null;

            foreach (var offerDomainEvent in offer.DomainEvents)
            {
                if (!(offerDomainEvent is IPartialDomainEvent)) domainEvents.Add(offerDomainEvent);

                switch (offerDomainEvent)
                {
                    case PriceChangedDomainEvent priceChangedEvent:
                        activeOfferChanged ??= new ActiveOfferChangedDomainEvent(offer.Id);
                        activeOfferChanged.PriceChange = priceChangedEvent.PriceChange;
                        break;
                    case MainImageChangedDomainEvent mainImageChanged:
                        activeOfferChanged ??= new ActiveOfferChangedDomainEvent(offer.Id);
                        activeOfferChanged.MainImageUriChange = mainImageChanged.MainImageUriChange;
                        break;
                    case AvailableStockChangedDomainEvent availableStockChanged:
                        activeOfferChanged ??= new ActiveOfferChangedDomainEvent(offer.Id);
                        activeOfferChanged.AvailableStockChange = availableStockChanged.AvailableStockChange;
                        break;
                    case NameChangedDomainEvent nameChanged:
                        activeOfferChanged ??= new ActiveOfferChangedDomainEvent(offer.Id);
                        activeOfferChanged.NameChange = nameChanged.NameChange;
                        break;
                }
            }

            if (activeOfferChanged != null) domainEvents.Add(activeOfferChanged);

            return domainEvents;
        }
    }
}
