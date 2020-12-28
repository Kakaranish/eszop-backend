using System;
using Common.EventBus;
using Common.IntegrationEvents;
using System.Threading.Tasks;
using Carts.API.DataAccess.Repositories;

namespace Carts.API.Application.IntegrationEventsHandlers
{
    public class OrderStartedIntegrationEventHandler : IntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly ICartRepository _cartRepository;

        public OrderStartedIntegrationEventHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public override async Task Handle(OrderStartedIntegrationEvent @event)
        {
            var cart = await _cartRepository.GetOrCreateByUserIdAsync(@event.UserId);
            
            _cartRepository.Delete(cart);
            await _cartRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();
        }
    }
}
