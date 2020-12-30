using MediatR;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;

namespace Orders.API.Application.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventBus _eventBus;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                OrderItems = request.CartItems.Select(item => new OrderItem
                {
                    OfferId = item.OfferId,
                    OfferName = item.OfferName,
                    PricePerItem = item.PricePerItem,
                    Quantity = item.Quantity
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            foreach (var orderItem in order.OrderItems)
            {
                var @event = new OrderStartedIntegrationEvent
                {
                    OfferId = orderItem.OfferId,
                    Quantity = orderItem.Quantity
                };
                await _eventBus.PublishAsync(@event);
            }
            
            return order.Id;
        }
    }
}
