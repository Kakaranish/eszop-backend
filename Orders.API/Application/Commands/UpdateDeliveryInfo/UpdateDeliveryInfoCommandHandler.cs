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
using Orders.API.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Commands.UpdateDeliveryInfo
{
    public class UpdateDeliveryInfoCommandHandler : IRequestHandler<UpdateDeliveryInfoCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IGrpcServiceClientFactory<IOffersService> _offersServiceClientFactory;
        private readonly ServicesEndpointsConfig _endpointsConfig;

        public UpdateDeliveryInfoCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository, IGrpcServiceClientFactory<IOffersService> offersServiceClientFactory,
            IOptions<ServicesEndpointsConfig> options)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _offersServiceClientFactory = offersServiceClientFactory ?? throw new ArgumentNullException(nameof(offersServiceClientFactory));
            _endpointsConfig = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public async Task<Unit> Handle(UpdateDeliveryInfoCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var orderId = Guid.Parse(request.OrderId);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.BuyerId != userId) throw new NotFoundException();

            var deliveryAddress = new DeliveryAddress(request.FirstName, request.LastName, request.PhoneNumber,
            request.Country, request.City, request.ZipCode, request.Street);
            order.SetDeliveryAddress(deliveryAddress);

            var offerIds = order.OrderItems.Select(orderItem => orderItem.Id);

            var offersServiceClient = _offersServiceClientFactory.Create(_endpointsConfig.Offers.Grpc.ToString());
            var grpcRequest = new GetDeliveryMethodsForOffersRequest { OfferIds = offerIds };
            var grpcResponse = await offersServiceClient.GetDeliveryMethodsForOffers(grpcRequest);

            var matchingDeliveryMethod = grpcResponse.DeliveryMethods?.FirstOrDefault(x => x.Name == request.DeliveryMethodName);
            if (matchingDeliveryMethod == null) throw new OrdersDomainException("Invalid delivery method name");

            var deliveryMethod = new DeliveryMethod(matchingDeliveryMethod.Name, matchingDeliveryMethod.Price);
            order.SetDeliveryMethod(deliveryMethod);

            _orderRepository.Update(order);
            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
