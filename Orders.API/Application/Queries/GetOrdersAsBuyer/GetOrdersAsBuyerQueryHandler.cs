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

namespace Orders.API.Application.Queries.GetOrdersAsBuyer
{
    public class GetOrdersAsBuyerQueryHandler : IRequestHandler<GetOrdersAsBuyerQuery, Pagination<OrderPreviewDto>>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;

        public GetOrdersAsBuyerQueryHandler(IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Pagination<OrderPreviewDto>> Handle(GetOrdersAsBuyerQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var ordersPagination = await _orderRepository.GetAllByBuyerIdAsync(userId, request.Filter);
            var ordersPreviewDtoPagination = ordersPagination.Transform(
                orders => orders.Select(order => order.ToPreviewDto()));

            return ordersPreviewDtoPagination;
        }
    }
}
