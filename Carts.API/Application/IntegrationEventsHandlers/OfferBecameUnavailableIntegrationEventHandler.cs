using Carts.API.DataAccess.Repositories;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using System;
using System.Threading.Tasks;

namespace Carts.API.Application.IntegrationEventsHandlers
{
    public class OfferBecameUnavailableIntegrationEventHandler : IntegrationEventHandler<OfferBecameUnavailableIntegrationEvent>
    {
        private readonly ICartItemRepository _cartItemRepository;

        public OfferBecameUnavailableIntegrationEventHandler(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
        }

        public override async Task Handle(OfferBecameUnavailableIntegrationEvent @event)
        {
            _cartItemRepository.RemoveWithOfferId(@event.OfferId);
            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();
        }
    }
}
