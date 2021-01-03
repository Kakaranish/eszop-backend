using Carts.API.DataAccess.Repositories;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using System;
using System.Threading.Tasks;

namespace Carts.API.Application.IntegrationEventsHandlers
{
    public class UserEndedOfferIntegrationEventHandler : IntegrationEventHandler<UserEndedOfferIntegrationEvent>
    {
        private readonly ICartItemRepository _cartItemRepository;

        public UserEndedOfferIntegrationEventHandler(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
        }

        public override async Task Handle(UserEndedOfferIntegrationEvent @event)
        {
            _cartItemRepository.RemoveWithOfferId(@event.OfferId);
            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();
        }
    }
}
