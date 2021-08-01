using Common.Domain.Types;
using Common.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.Application.Dto;
using Orders.API.Extensions;
using Orders.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Queries.GetOrdersAsSeller
{
    public class GetOrdersAsSellerQueryHandler : IRequestHandler<GetOrdersAsSellerQuery, Pagination<OrderPreviewDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;

        public GetOrdersAsSellerQueryHandler(IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Pagination<OrderPreviewDto>> Handle(GetOrdersAsSellerQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var ordersPagination = await _orderRepository.GetAllBySellerIdAsync(userId, request.Filter);

            var ordersPreviewDtoPagination = ordersPagination.Transform(
                orders => orders.Select(order => order.ToPreviewDto()));

            return ordersPreviewDtoPagination;
        }
    }
}
