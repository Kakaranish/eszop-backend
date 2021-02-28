using Common.Dto;
using Common.Exceptions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.Application.Services;
using Orders.API.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Queries.GetAvailableDeliveryMethodsForOrder
{
    public class GetAvailableDeliveryMethodsForOrderQueryHandler
        : IRequestHandler<GetAvailableDeliveryMethodsForOrderQuery, IList<DeliveryMethodDto>>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryMethodsProvider _deliveryMethodsProvider;

        public GetAvailableDeliveryMethodsForOrderQueryHandler(IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository, IDeliveryMethodsProvider deliveryMethodsProvider)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _deliveryMethodsProvider = deliveryMethodsProvider ?? throw new ArgumentNullException(nameof(deliveryMethodsProvider));
        }

        public async Task<IList<DeliveryMethodDto>> Handle(GetAvailableDeliveryMethodsForOrderQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var orderId = Guid.Parse(request.OrderId);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order.BuyerId != userId) throw new NotFoundException();

            var deliveryMethods = await _deliveryMethodsProvider.Get(order);

            return deliveryMethods;
        }
    }
}
