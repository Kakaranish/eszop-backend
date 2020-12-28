using Common.EventBus;
using Common.IntegrationEvents;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.API.Application.IntegrationEventHandlers
{
    public class CartFinalizedIntegrationEventHandler : IntegrationEventHandler<CartFinalizedIntegrationEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventBus _eventBus;

        public CartFinalizedIntegrationEventHandler(IOrderRepository orderRepository, IEventBus eventBus)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public override async Task Handle(CartFinalizedIntegrationEvent @event)
        {
            var order = new Order
            {
                OrderItems = @event.CartItems.Select(item => new OrderItem
                {
                    OfferId = item.OfferId,
                    OfferName = item.OfferName,
                    PricePerItem = item.PricePerItem,
                    Quantity = item.Quantity
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            var integrationEvent = new OrderStartedIntegrationEvent
            {
                UserId = @event.UserId,
                OrderId = order.Id
            };
            await _eventBus.PublishAsync(integrationEvent);
        }
    }
}
