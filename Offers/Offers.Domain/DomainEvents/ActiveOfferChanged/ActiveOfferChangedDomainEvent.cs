using Common.Domain.DomainEvents;
using Common.Domain.Types;
using System;

namespace Offers.Domain.DomainEvents.ActiveOfferChanged
{
    public class ActiveOfferChangedDomainEvent : IDomainEvent
    {
        public Guid OfferId { get; }
        public ChangeState<string> NameChange { get; set; }
        public ChangeState<decimal> PriceChange { get; set; }
        public ChangeState<int> AvailableStockChange { get; set; }
        public ChangeState<string> MainImageUriChange { get; set; }

        public ActiveOfferChangedDomainEvent(Guid offerId)
        {
            OfferId = offerId;
        }
    }
}
