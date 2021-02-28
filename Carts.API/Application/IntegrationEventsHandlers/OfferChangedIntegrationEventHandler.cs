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
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(OfferChangedIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OfferId".ToKvp(@event.OfferId));

            await _cartItemRepository.UpdateWithOfferChangedEvent(@event);
            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            _logger.LogWithProps(LogLevel.Information,
                $"Handled {nameof(OfferChangedIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OfferId".ToKvp(@event.OfferId));
        }
    }
}
