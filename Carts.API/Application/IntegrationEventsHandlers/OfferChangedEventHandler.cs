using System;
using Common.IntegrationEvents;
using Common.ServiceBus;
using System.Threading.Tasks;
using Carts.API.DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace Carts.API.Application.IntegrationEventsHandlers
{
    public class OfferChangedEventHandler : IntegrationEventHandler<OfferChangedEvent>
    {
        private readonly ILogger<OfferChangedEventHandler> _logger;
        private readonly ICartItemRepository _cartItemRepository;

        public OfferChangedEventHandler(ILogger<OfferChangedEventHandler> logger, ICartItemRepository cartItemRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
        }

        public override async Task Handle(OfferChangedEvent integrationEvent)
        {
            _logger.LogInformation($"Handling {nameof(OfferChangedEvent)} event for offer {integrationEvent.Id}");

            await _cartItemRepository.UpdateWithOfferChangedEvent(integrationEvent);
            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();
        }
    }
}
