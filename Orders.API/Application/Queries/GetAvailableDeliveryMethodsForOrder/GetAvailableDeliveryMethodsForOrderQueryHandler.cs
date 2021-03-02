using Common.Exceptions;
using Common.Extensions;
using Common.Grpc;
using Common.Grpc.Services.OffersService;
using Common.Grpc.Services.OffersService.Requests.GetDeliveryMethodsForOffers;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Orders.API.DataAccess.Repositories;
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
        private readonly IGrpcServiceClientFactory<IOffersService> _offersServiceClientFactory;
        private readonly ServicesEndpointsConfig _endpointsConfig;

        public GetAvailableDeliveryMethodsForOrderQueryHandler(IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository, IGrpcServiceClientFactory<IOffersService> offersServiceClientFactory,
            IOptions<ServicesEndpointsConfig> options)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _offersServiceClientFactory = offersServiceClientFactory ?? throw new ArgumentNullException(nameof(offersServiceClientFactory));
            _endpointsConfig = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public async Task<IList<DeliveryMethodDto>> Handle(GetAvailableDeliveryMethodsForOrderQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var orderId = Guid.Parse(request.OrderId);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order.BuyerId != userId) throw new NotFoundException();

            var offerIds = order.OrderItems.Select(orderItem => orderItem.OfferId);

            var offersServiceClient = _offersServiceClientFactory.Create(_endpointsConfig.Offers.Grpc.ToString());
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
