using Common.Domain.DomainEvents;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Logging;
using Common.Utilities.Types;
using Microsoft.Extensions.Logging;
using Orders.Domain.Aggregates.OrderAggregate;
using Orders.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.DomainEventHandlers
{
    public class OrderCancelledDomainEventHandler : IDomainEventHandler<OrderCancelledDomainEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<OrderCancelledDomainEventHandler> _logger;

        public OrderCancelledDomainEventHandler(IEventBus eventBus, ILogger<OrderCancelledDomainEventHandler> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(OrderCancelledDomainEvent)} domain event",
                "OrderId".ToKvp(domainEvent.OrderId));

            var integrationEvent = new OrderCancelledIntegrationEvent
            {
                OrderId = domainEvent.OrderId,
                OrderItems = domainEvent.OrderItems,
                PreviousState = domainEvent.PreviousState.Name,
                CurrentState = domainEvent.CurrentState.Name
            };

            await _eventBus.PublishAsync(integrationEvent);

            _logger.LogWithProps(LogLevel.Information,
                $"Integration event {nameof(OrderCancelledIntegrationEvent)} published",
                "EventId".ToKvp(integrationEvent.Id),
                "PreviousState".ToKvp(integrationEvent.PreviousState),
                "CurrentState".ToKvp(integrationEvent.CurrentState),
                "OfferIds".ToKvp(string.Join(",", integrationEvent.OrderItems.Select(x => x.OfferId))));

            var notificationIntegrationEvents = PrepareNotificationIntegrationEvents(domainEvent);
            foreach (var notificationIntegrationEvent in notificationIntegrationEvents)
            {
                await _eventBus.PublishAsync(notificationIntegrationEvent);
            }

            _logger.LogWithProps(LogLevel.Information,
                $"Published {notificationIntegrationEvents.Count} {nameof(OrderCancelledIntegrationEvent)} integration events",
                "PublishedEventIds".ToKvp(string.Join(",", notificationIntegrationEvents.Select(x => x.Id))),
                "ReceiverIds".ToKvp(string.Join(",", notificationIntegrationEvents.Select(x => x.UserId))));
        }

        private IList<NotificationIntegrationEvent> PrepareNotificationIntegrationEvents(
            OrderCancelledDomainEvent domainEvent)
        {
            var metadata = new Dictionary<string, string>
            {
                {"PreviousOrderState", domainEvent.PreviousState.Name},
                {"CurrentOrderState", domainEvent.CurrentState.Name},
                {"OrderId", domainEvent.OrderId.ToString()}
            };
            var message = "Order cancelled by seller";

            var notificationIntegrationEvents = new List<NotificationIntegrationEvent>();
            if (domainEvent.CurrentState == OrderState.CancelledBySeller)
            {
                notificationIntegrationEvents.Add(new NotificationIntegrationEvent
                {
                    UserId = domainEvent.BuyerId,
                    Code = NotificationCodes.OrderCancelledBySeller,
                    Message = message,
                    Metadata = metadata
                });
            }
            else if (domainEvent.CurrentState == OrderState.CancelledByBuyer)
            {
                notificationIntegrationEvents.Add(new NotificationIntegrationEvent
                {
                    UserId = domainEvent.SellerId,
                    Code = NotificationCodes.OrderCancelledByBuyer,
                    Message = message,
                    Metadata = metadata
                });
            }
            else
            {
                notificationIntegrationEvents.Add(new NotificationIntegrationEvent
                {
                    UserId = domainEvent.BuyerId,
                    Code = NotificationCodes.OrderCancelled,
                    Message = message,
                    Metadata = metadata
                });
                notificationIntegrationEvents.Add(new NotificationIntegrationEvent
                {
                    UserId = domainEvent.SellerId,
                    Code = NotificationCodes.OrderCancelled,
                    Message = message,
                    Metadata = metadata
                });
            }

            return notificationIntegrationEvents;
        }
    }
}
