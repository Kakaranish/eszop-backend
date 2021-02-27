using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.Application.Dto;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using Orders.API.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Queries.GetOrderSummary
{
    public class GetOrderSummaryQueryHandler : IRequestHandler<GetOrderSummaryQuery, OrderDto>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;

        public GetOrderSummaryQueryHandler(IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<OrderDto> Handle(GetOrderSummaryQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var orderId = Guid.Parse(request.OrderId);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.BuyerId != userId)
                throw new OrdersDomainException($"There is no order with id {orderId}");

            return order.ToDto();
        }
    }
}
