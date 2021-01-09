using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
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
            if (order == null) throw new OrdersDomainException($"Order {orderId} not found");

            var userClaims = _httpContext.User.Claims.ToTokenPayload().UserClaims;
            var userId = userClaims.Id;
            var userRole = userClaims.Role;

            if (userId == order.BuyerId) order.SetCancelled(OrderState.CancelledByBuyer);
            else if (userId == order.SellerId) order.SetCancelled(OrderState.CancelledBySeller);
            else if (userRole.ToLowerInvariant() == "admin") order.SetCancelled(OrderState.Cancelled);
            else throw new OrdersDomainException($"Order {orderId} not found");

            _orderRepository.Update(order);
            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
