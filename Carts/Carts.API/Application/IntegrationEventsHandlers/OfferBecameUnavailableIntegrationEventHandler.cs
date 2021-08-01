using Carts.Domain.Aggregates.Repositories;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Logging;
using Common.Utilities.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carts.API.Application.IntegrationEventsHandlers
{
    public class OfferBecameUnavailableIntegrationEventHandler : IntegrationEventHandler<OfferBecameUnavailableIntegrationEvent>
    {
        private readonly ILogger<OfferBecameUnavailableIntegrationEventHandler> _logger;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IEventBus _eventBus;

        public OfferBecameUnavailableIntegrationEventHandler(
            ILogger<OfferBecameUnavailableIntegrationEventHandler> logger,
            ICartItemRepository cartItemRepository, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public override async Task Handle(OfferBecameUnavailableIntegrationEvent @event)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(OfferBecameUnavailableIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OfferId".ToKvp(@event.OfferId));

            var cartItems = await _cartItemRepository.GetByOfferId(@event.OfferId);
            if (!cartItems.Any()) return;

            var notifications = new List<NotificationIntegrationEvent>();
            foreach (var cartItem in cartItems)
            {
                var notification = new NotificationIntegrationEvent
                {
                    UserId = cartItem.CartOwnerId,
                    Code = NotificationCodes.CartItemBecameUnavailable,
                    Message = "Offer became unavailable",
                    Metadata = new Dictionary<string, string>
                    {
                        {"OfferId", cartItem.OfferId.ToString()},
                        {"OfferName", cartItem.OfferName},
                        {"CartItemId", cartItem.Id.ToString()}
                    }
                };
                notifications.Add(notification);

                _cartItemRepository.Remove(cartItem);
            }
            _cartItemRepository.RemoveWithOfferId(@event.OfferId);
            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            foreach (var notification in notifications)
            {
                await _eventBus.PublishAsync(notification);
            }

            _logger.LogWithProps(LogLevel.Information,
                $"Published {notifications.Count} {nameof(NotificationIntegrationEvent)}",
                "SourceEventId".ToKvp(@event.Id),
                "OfferId".ToKvp(@event.OfferId));
        }
    }
}
