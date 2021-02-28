using Common.Domain;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.DomainEvents.AvailableStockChanged
{
    public class AvailableStockChangedDomainEventHandler : IDomainEventHandler<AvailableStockChangedDomainEvent>
    {
        private readonly ILogger<AvailableStockChangedDomainEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public AvailableStockChangedDomainEventHandler(ILogger<AvailableStockChangedDomainEventHandler> logger,
            IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(AvailableStockChangedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(AvailableStockChangedDomainEvent)} domain event",
                "OfferId".ToKvp(domainEvent.OfferId),
                "PreviousValue".ToKvp(domainEvent.AvailableStock.OldValue),
                "CurrentValue".ToKvp(domainEvent.AvailableStock.NewValue));

            var integrationEvent = new OfferChangedIntegrationEvent
            {
                OfferId = domainEvent.OfferId,
                AvailableStock = domainEvent.AvailableStock
            };

            await _eventBus.PublishAsync(integrationEvent);

            _logger.LogWithProps(LogLevel.Information,
                $"Published {nameof(OfferChangedIntegrationEvent)} integration event",
                "EventId".ToKvp(integrationEvent.Id),
                "OfferId".ToKvp(integrationEvent.OfferId));
        }
    }
}
