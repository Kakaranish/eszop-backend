using Common.Extensions;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.Application.Dto;
using Orders.API.DataAccess.Repositories;
using Orders.API.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Queries
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Pagination<OrderPreviewDto>>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;

        public GetOrdersQueryHandler(IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Pagination<OrderPreviewDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var ordersPagination = await _orderRepository.GetAllByUserIdAsync(userId, request.Filter);
            var ordersPreviewDtoPagination = ordersPagination.Transform(
                orders => orders.Select(order => order.ToPreviewDto()));

            return ordersPreviewDtoPagination;
        }
    }
}
