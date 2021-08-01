using Common.Domain.DomainEvents;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Logging;
using Microsoft.Extensions.Logging;
using Offers.Domain.DomainEvents.ActiveOfferChanged;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.DomainEventHandlers.ActiveOfferChanged
{
    public class ActiveOfferChangedDomainEventHandler : IDomainEventHandler<ActiveOfferChangedDomainEvent>
    {
        private readonly ILogger<ActiveOfferChangedDomainEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public ActiveOfferChangedDomainEventHandler(ILogger<ActiveOfferChangedDomainEventHandler> logger,
            IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(ActiveOfferChangedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(ActiveOfferChangedDomainEvent)} domain event",
                "OfferId".ToKvp(domainEvent.OfferId));

            var integrationEvent = new ActiveOfferChangedIntegrationEvent
            {
                OfferId = domainEvent.OfferId,
                PriceChange = domainEvent.PriceChange,
                NameChange = domainEvent.NameChange,
                AvailableStockChange = domainEvent.AvailableStockChange,
                MainImageUriChange = domainEvent.MainImageUriChange
            };

            await _eventBus.PublishAsync(integrationEvent);

            _logger.LogWithProps(LogLevel.Information,
                $"Published {nameof(ActiveOfferChangedIntegrationEvent)} integration event",
                "EventId".ToKvp(integrationEvent.Id),
                "OfferId".ToKvp(integrationEvent.OfferId));
        }
    }
}
