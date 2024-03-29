﻿using Common.Domain.DomainEvents;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Logging;
using Microsoft.Extensions.Logging;
using Offers.Domain.DomainEvents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.DomainEventHandlers
{
    public class OfferBecameUnavailableDomainEventHandler : IDomainEventHandler<OfferBecameUnavailableDomainEvent>
    {
        private readonly ILogger<OfferBecameUnavailableDomainEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public OfferBecameUnavailableDomainEventHandler(ILogger<OfferBecameUnavailableDomainEventHandler> logger,
            IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(OfferBecameUnavailableDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(OfferBecameUnavailableDomainEvent)} domain event",
                "OfferId".ToKvp(domainEvent.OfferId));

            var integrationEvent = new OfferBecameUnavailableIntegrationEvent
            {
                OfferId = domainEvent.OfferId,
                Trigger = domainEvent.Trigger
            };

            await _eventBus.PublishAsync(integrationEvent);

            _logger.LogWithProps(LogLevel.Information,
                $"Integration event {nameof(OfferBecameUnavailableIntegrationEvent)} published",
                "EventId".ToKvp(integrationEvent.Id),
                "OfferId".ToKvp(domainEvent.OfferId));
        }
    }
}
