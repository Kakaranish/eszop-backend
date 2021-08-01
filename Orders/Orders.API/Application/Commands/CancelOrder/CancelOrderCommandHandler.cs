using Common.Utilities.Exceptions;
using Common.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.Domain.Aggregates.OrderAggregate;
using Orders.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;

        public CancelOrderCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var orderId = Guid.Parse(request.OrderId);
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException();

            var userClaims = _httpContext.User.Claims.ToTokenPayload().UserClaims;
            var userId = userClaims.Id;
            var userRole = userClaims.Role;

            if (userId == order.BuyerId) order.SetCancelled(OrderState.CancelledByBuyer);
            else if (userId == order.SellerId) order.SetCancelled(OrderState.CancelledBySeller);
            else if (userRole.ToUpperInvariant() == "ADMIN") order.SetCancelled(OrderState.Cancelled);
            else throw new NotFoundException();

            _orderRepository.Update(order);
            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
