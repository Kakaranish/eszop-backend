using Common.Exceptions;
using Common.Extensions;
using Common.Grpc.Services.OffersService.Requests.GetDeliveryMethodsForOffers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.DataAccess.Repositories;
using Orders.API.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryMethodDto = Common.Dto.DeliveryMethodDto;

namespace Orders.API.Application.Queries.GetAvailableDeliveryMethodsForOrder
{
    public class GetAvailableDeliveryMethodsForOrderQueryHandler
        : IRequestHandler<GetAvailableDeliveryMethodsForOrderQuery, IList<DeliveryMethodDto>>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IOffersServiceClientFactory _offersServiceClientFactory;

        public GetAvailableDeliveryMethodsForOrderQueryHandler(IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository, IOffersServiceClientFactory offersServiceClientFactory)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _offersServiceClientFactory = offersServiceClientFactory ?? throw new ArgumentNullException(nameof(offersServiceClientFactory));
        }

        public async Task<IList<DeliveryMethodDto>> Handle(GetAvailableDeliveryMethodsForOrderQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var orderId = Guid.Parse(request.OrderId);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order.BuyerId != userId) throw new NotFoundException();

            var offerIds = order.OrderItems.Select(orderItem => orderItem.OfferId);

            var offersServiceClient = _offersServiceClientFactory.Create();
            var grpcRequest = new GetDeliveryMethodsForOffersRequest { OfferIds = offerIds };
            var grpcResponse = await offersServiceClient.GetDeliveryMethodsForOffers(grpcRequest);

            return grpcResponse.DeliveryMethods.Select(method => new DeliveryMethodDto
            {
                Name = method.Name,
                Price = method.Price
            }).ToList();
        }
    }
}
