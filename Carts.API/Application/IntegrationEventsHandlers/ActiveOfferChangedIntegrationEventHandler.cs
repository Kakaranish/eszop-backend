using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Logging;
using Common.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Carts.API.Application.IntegrationEventsHandlers
{
    public class ActiveOfferChangedIntegrationEventHandler : IntegrationEventHandler<ActiveOfferChangedIntegrationEvent>
    {
        private readonly ILogger<ActiveOfferChangedIntegrationEvent> _logger;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IEventBus _eventBus;

        public ActiveOfferChangedIntegrationEventHandler(ILogger<ActiveOfferChangedIntegrationEvent> logger,
            ICartItemRepository cartItemRepository, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public override async Task Handle(ActiveOfferChangedIntegrationEvent @event)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(ActiveOfferChangedIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OfferId".ToKvp(@event.OfferId));

            if (!(@event.PriceChange?.Changed ?? false) && !(@event.AvailableStockChange?.Changed ?? false) &&
                !(@event.NameChange?.Changed ?? false) && !(@event.MainImageUriChange?.Changed ?? false))
            {
                _logger.LogWithProps(LogLevel.Information,
                    $"No change was included in integration event",
                    "EventId".ToKvp(@event.Id),
                    "OfferId".ToKvp(@event.OfferId));

                return;
            }

            var notificationIntegrationEvents = new List<NotificationIntegrationEvent>();

            var cartItems = await _cartItemRepository.GetByOfferId(@event.OfferId);
            foreach (var cartItem in cartItems)
            {
                if (@event.PriceChange?.Changed ?? false) cartItem.SetPricePerItem(@event.PriceChange.NewValue);
                if (@event.AvailableStockChange?.Changed ?? false) cartItem.SetAvailableStock(@event.AvailableStockChange.NewValue);
                if (@event.NameChange?.Changed ?? false) cartItem.SetOfferName(@event.NameChange.NewValue);
                if (@event.MainImageUriChange?.Changed ?? false) cartItem.SetImageUri(@event.MainImageUriChange.NewValue);

                _cartItemRepository.Update(cartItem);

                notificationIntegrationEvents.Add(new NotificationIntegrationEvent
                {
                    UserId = cartItem.CartOwnerId,
                    Code = NotificationCodes.CartItemChanged,
                    Message = "Cart item changed",
                    Metadata = PrepareMetadata(@event, cartItem)
                });
            }

            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            foreach (var notificationIntegrationEvent in notificationIntegrationEvents)
            {
                await _eventBus.PublishAsync(notificationIntegrationEvent);
            }

            _logger.LogWithProps(LogLevel.Information,
                $"Published {notificationIntegrationEvents.Count} {nameof(NotificationIntegrationEvent)} integration events",
                "EventId".ToKvp(@event.Id),
                "PublishedEventIds".ToKvp(string.Join(",", notificationIntegrationEvents.Select(x => x.Id))),
                "OfferId".ToKvp(@event.OfferId));
        }

        private static IDictionary<string, string> PrepareMetadata(
            ActiveOfferChangedIntegrationEvent integrationEvent, CartItem cartItem)
        {
            var metadata = new Dictionary<string, string>
            {
                {"CartItemId", cartItem.CartOwnerId.ToString()}
            };

            if (integrationEvent.AvailableStockChange?.Changed ?? false)
                metadata.Add("AvailableStock", integrationEvent.AvailableStockChange.NewValue.ToString());
            if (integrationEvent.PriceChange?.Changed ?? false)
                metadata.Add("Price", integrationEvent.PriceChange.NewValue.ToString(CultureInfo.InvariantCulture));

            return metadata;
        }
    }
}
