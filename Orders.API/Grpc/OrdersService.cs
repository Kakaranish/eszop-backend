using Common.Dto;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Grpc.Services.OrdersService;
using Common.Grpc.Services.OrdersService.CreateOrder;
using Microsoft.Extensions.Logging;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.API.Grpc
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
            var orderItems = request.CartItems.Select(item => new OrderItem(
                item.OfferId, item.OfferName, item.Quantity, item.PricePerItem, item.ImageUri)).ToList();
            var order = new Order(request.UserId, request.SellerId, orderItems);

            _orderRepository.Add(order);
            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            var integrationEvent = new OrderStartedIntegrationEvent
            {
                OrderId = order.Id,
                OrderItems = order.OrderItems.Select(orderItem => new OrderItemDto
                {
                    OfferId = orderItem.OfferId,
                    Quantity = orderItem.Quantity
                }).ToList()
            };
            await _eventBus.PublishAsync(integrationEvent);

            _logger.LogInformation($"Integration event {nameof(OrderStartedIntegrationEvent)} published",
                "EventId".ToKvp(integrationEvent.Id),
                "OfferIds".ToKvp(string.Join(",", integrationEvent.OrderItems.Select(x => x.OfferId))));

            return new CreateOrderResponse { OrderId = order.Id };
        }
    }
}
