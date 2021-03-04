using Carts.API.DataAccess.Repositories;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Carts.API.Application.IntegrationEventsHandlers
{
    public class OfferBecameUnavailableIntegrationEventHandler : IntegrationEventHandler<OfferBecameUnavailableIntegrationEvent>
    {
        private readonly ILogger<OfferBecameUnavailableIntegrationEventHandler> _logger;
        private readonly ICartItemRepository _cartItemRepository;

        public OfferBecameUnavailableIntegrationEventHandler(
            ILogger<OfferBecameUnavailableIntegrationEventHandler> logger, ICartItemRepository cartItemRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
        }

        public override async Task Handle(OfferBecameUnavailableIntegrationEvent @event)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(OfferBecameUnavailableIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OfferId".ToKvp(@event.OfferId));

            _cartItemRepository.RemoveWithOfferId(@event.OfferId); //TODO? Notify user
            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            _logger.LogWithProps(LogLevel.Information,
                $"Handled {nameof(OfferBecameUnavailableIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OfferId".ToKvp(@event.OfferId));
        }
    }
}
