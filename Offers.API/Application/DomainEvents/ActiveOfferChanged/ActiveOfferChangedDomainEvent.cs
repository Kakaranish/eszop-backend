using Common.Domain;
using Common.Types;

namespace Offers.API.Application.DomainEvents.ActiveOfferChanged
{
    public class ActiveOfferChangedDomainEvent : IDomainEvent
    {
        public ChangeState<string> NameChange { get; set; }
        public ChangeState<decimal> PriceChange { get; set; }
        public ChangeState<int> AvailableStockChange { get; set; }
        public ChangeState<string> MainImageUriChange { get; set; }
    }
}
