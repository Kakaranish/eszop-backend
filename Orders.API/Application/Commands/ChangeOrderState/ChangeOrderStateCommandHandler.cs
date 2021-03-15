using Common.Exceptions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Commands.ChangeOrderState
{
    public class ChangeOrderStateCommandHandler : IRequestHandler<ChangeOrderStateCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;

        public ChangeOrderStateCommandHandler(
            IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Unit> Handle(ChangeOrderStateCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var orderId = Guid.Parse(request.OrderId);
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.SellerId != userId) throw new NotFoundException();

            var newOrderState = OrderState.Parse(request.OrderState);
            if (newOrderState == order.OrderState) return await Unit.Task;

            order.ChangeOrderState(newOrderState);

            _orderRepository.Update(order);
            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
