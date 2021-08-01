using Common.Utilities.Exceptions;
using Common.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.Application.Dto;
using Orders.API.Extensions;
using Orders.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Queries.GetDeliveryInfo
{
    public class GetDeliveryInfoQueryHandler : IRequestHandler<GetDeliveryInfoQuery, DeliveryInfoDto>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;

        public GetDeliveryInfoQueryHandler(IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<DeliveryInfoDto> Handle(GetDeliveryInfoQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var orderId = Guid.Parse(request.OrderId);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.BuyerId != userId) throw new NotFoundException();

            return new DeliveryInfoDto
            {
                DeliveryAddress = order.DeliveryAddress.ToDto(),
                DeliveryMethod = order.DeliveryMethod.ToDto()
            };
        }
    }
}
