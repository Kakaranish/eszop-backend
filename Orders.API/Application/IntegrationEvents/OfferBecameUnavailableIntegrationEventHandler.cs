using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Logging;
using Common.Types;
using Microsoft.Extensions.Logging;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.API.Application.IntegrationEvents
{
    public class OfferBecameUnavailableIntegrationEventHandler : IntegrationEventHandler<OfferBecameUnavailableIntegrationEvent>
    {
        private readonly ILogger<OfferBecameUnavailableIntegrationEventHandler> _logger;
        private readonly IEventBus _eventBus;
        private readonly IOrderRepository _orderRepository;

        public OfferBecameUnavailableIntegrationEventHandler(
            ILogger<OfferBecameUnavailableIntegrationEventHandler> logger, IEventBus eventBus,
            IOrderRepository orderRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public override async Task Handle(OfferBecameUnavailableIntegrationEvent @event)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(OfferBecameUnavailableIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OfferId".ToKvp(@event.OfferId));

            var ordersToCancel = await _orderRepository.GetAllStartedOrdersByOfferId(@event.OfferId);
            foreach (var order in ordersToCancel)
            {
                order.SetCancelled(OrderState.CancelledBySeller);
            }

            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            _logger.LogWithProps(LogLevel.Information,
                $"Cancelled orders",
                "EventId".ToKvp(@event.Id),
                "OrderIds".ToKvp(string.Join(",", ordersToCancel.Select(x => x.Id))));

            foreach (var orderToCancel in ordersToCancel)
            {
                var notification = new NotificationIntegrationEvent
                {
                    UserId = orderToCancel.BuyerId,
                    Code = NotificationCodes.OfferBecameUnavailable,
                    Message = "Offer became unavailable",
                    Metadata = new Dictionary<string, string>
                    {
                        {"OrderId",  orderToCancel.Id.ToString()},
                        {"SourceOfferId", @event.OfferId.ToString() }
                    }
                };

                await _eventBus.PublishAsync(notification);
            }

            _logger.LogWithProps(LogLevel.Information,
                $"Published {ordersToCancel.Count} notifications",
                "EventId".ToKvp(@event.Id),
                "AffectedOrderIds".ToKvp(string.Join(",", ordersToCancel.Select(x => x.Id))));
        }
    }
}
