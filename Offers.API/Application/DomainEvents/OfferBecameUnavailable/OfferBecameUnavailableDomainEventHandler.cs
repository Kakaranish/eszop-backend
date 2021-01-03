using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.DomainEvents.OfferBecameUnavailable
{
    public class OfferBecameUnavailableDomainEventHandler : INotificationHandler<OfferBecameUnavailableDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public OfferBecameUnavailableDomainEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(OfferBecameUnavailableDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var integrationEvent = new OfferBecameUnavailableIntegrationEvent { OfferId = domainEvent.OfferId };

            await _eventBus.PublishAsync(integrationEvent);
        }
    }
}
