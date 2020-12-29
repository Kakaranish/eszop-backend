using Common.EventBus;
using Common.IntegrationEvents;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.DomainEvents.AvailableStockChanged
{
    public class AvailableStockChangedDomainEventHandler : INotificationHandler<AvailableStockChangedDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public AvailableStockChangedDomainEventHandler(IEventBus eventBus)
        {
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
        }
    }
}
