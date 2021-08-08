using Common.Dto;
using Common.Grpc.Services.OrdersService;
using Common.Grpc.Services.OrdersService.CreateOrder;
using Common.Grpc.Services.OrdersService.GetOfferHasOrders;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Types;
using Microsoft.Extensions.Logging;
using Orders.Domain.Aggregates.OrderAggregate;
using Orders.Domain.Aggregates.OrderItemAggregate;
using Orders.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Grpc
{
    public class OrdersService : IOrdersService
    {
        private readonly ILogger<OrdersService> _logger;
        private readonly IEventBus _eventBus;
        private readonly IOrderRepository _orderRepository;

        public OrdersService(ILogger<OrdersService> logger, IEventBus eventBus, IOrderRepository orderRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request)
        {
            var orderItems = new List<OrderItem>();
            foreach (var cartItem in request.CartItems)
            {
                var offerDetails = new OfferDetails(cartItem.OfferId, cartItem.OfferName, cartItem.PricePerItem, cartItem.ImageUri);
                var orderItem = new OrderItem(offerDetails, cartItem.Quantity);
                orderItems.Add(orderItem);
            }

            var buyer = new Buyer(request.UserId);
            var order = new Order(buyer, request.SellerId, orderItems);

            _orderRepository.Add(order);
            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent
            {
                OrderId = order.Id,
                OrderItems = order.OrderItems.Select(orderItem => new OrderItemDto
                {
                    OfferId = orderItem.OfferDetails.Id,
                    Quantity = orderItem.Quantity
                }).ToList()
            };
            await _eventBus.PublishAsync(orderStartedIntegrationEvent);

            _logger.LogInformation($"Integration event {nameof(OrderStartedIntegrationEvent)} published",
                "EventId".ToKvp(orderStartedIntegrationEvent.Id),
                "OfferIds".ToKvp(string.Join(",", orderStartedIntegrationEvent.OrderItems.Select(x => x.OfferId))));

            var notificationIntegrationEvent = new NotificationIntegrationEvent
            {
                UserId = order.SellerId,
                Code = NotificationCodes.OrderStarted,
                Message = "Order started",
                Metadata = new Dictionary<string, string>
                {
                    {"OrderId",  order.Id.ToString()},
                    {"BuyerId", order.Buyer.Id.ToString() },
                    {"OfferIds", string.Join(",", request.CartItems.Select(x => x.OfferId)) }
                }

            };
            await _eventBus.PublishAsync(notificationIntegrationEvent);

            _logger.LogInformation($"Published {nameof(NotificationIntegrationEvent)}",
                "EventId".ToKvp(orderStartedIntegrationEvent.Id),
                "PublishedEventId".ToKvp(notificationIntegrationEvent.Id),
                "NotifiedUserId".ToKvp(notificationIntegrationEvent.UserId));

            return new CreateOrderResponse { OrderId = order.Id };
        }

        public async Task<GetOfferHasOrdersResponse> GetOfferHasOrders(GetOfferHasOrdersRequest request)
        {
            var offerHasOrders = await _orderRepository.GetOfferHasAnyOrders(request.OfferId);
            return new GetOfferHasOrdersResponse
            {
                OfferHasOrders = offerHasOrders
            };
        }
    }
}
