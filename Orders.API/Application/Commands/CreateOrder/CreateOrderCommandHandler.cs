using Common.Dto;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventBus _eventBus;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus,
            ILogger<CreateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderItems = request.CartItems.Select(item => new OrderItem(
                item.OfferId, item.OfferName, item.Quantity, item.PricePerItem, item.ImageUri)).ToList();
            var order = new Order(request.BuyerId, request.SellerId, orderItems);

            _orderRepository.Add(order);
            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

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

            return order.Id;
        }
    }
}
