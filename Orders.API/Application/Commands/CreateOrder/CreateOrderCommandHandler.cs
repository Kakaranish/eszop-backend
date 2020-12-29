using MediatR;
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

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
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

            return order.Id;
        }
    }
}
