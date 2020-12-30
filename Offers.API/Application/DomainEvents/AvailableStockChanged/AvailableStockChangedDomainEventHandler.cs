using Common.EventBus;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Common.EventBus.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace Offers.API.Application.DomainEvents.AvailableStockChanged
{
    public class AvailableStockChangedDomainEventHandler : INotificationHandler<AvailableStockChangedDomainEvent>
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
            var integrationEvent = new OfferChangedIntegrationEvent
            {
                OfferId = domainEvent.OfferId,
                AvailableStock = domainEvent.AvailableStock
            };

            await _eventBus.PublishAsync(integrationEvent);
            _logger.LogInformation($"Published {nameof(OfferChangedIntegrationEvent)} integration event");
        }
    }
}
