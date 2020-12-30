using System;
using Common.IntegrationEvents;
using System.Threading.Tasks;
using Carts.API.DataAccess.Repositories;
using Common.EventBus;
using Microsoft.Extensions.Logging;

namespace Carts.API.Application.IntegrationEventsHandlers
{
    public class OfferChangedIntegrationEventHandler : IntegrationEventHandler<OfferChangedIntegrationEvent>
    {
        private readonly ILogger<OfferChangedIntegrationEventHandler> _logger;
        private readonly ICartItemRepository _cartItemRepository;

        public OfferChangedIntegrationEventHandler(ILogger<OfferChangedIntegrationEventHandler> logger, 
            ICartItemRepository cartItemRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cartItemRepository = cartItemRepository ?? 
                                  throw new ArgumentNullException(nameof(cartItemRepository));
        }

        public override async Task Handle(OfferChangedIntegrationEvent @event)
        {
            await _cartItemRepository.UpdateWithOfferChangedEvent(@event);
            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            _logger.LogInformation($"Handled {nameof(OfferChangedIntegrationEvent)} event for offer {@event.OfferId}");
        }
    }
}
