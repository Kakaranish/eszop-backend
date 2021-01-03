using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.DomainEvents.UserEndedOffer
{
    public class UserEndedOfferDomainEventHandler : INotificationHandler<UserEndedOfferDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public UserEndedOfferDomainEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(UserEndedOfferDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserEndedOfferIntegrationEvent { OfferId = domainEvent.OfferId };

            await _eventBus.PublishAsync(integrationEvent);
        }
    }
}
