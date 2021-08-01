using Common.Exceptions;
using Common.Extensions;
using Common.Grpc;
using Common.Grpc.Services.OrdersService;
using Common.Grpc.Services.OrdersService.GetOfferHasOrders;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Offers.Domain;
using Offers.Domain.Exceptions;
using Offers.Domain.Repositories;

namespace Offers.API.Application.Commands.RemoveOffer
{
    public class RemoveOfferCommandHandler : IRequestHandler<RemoveOfferCommand>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly HttpContext _httpContext;
        private readonly IGrpcServiceClientFactory<IOrdersService> _ordersServiceClientFactory;
        private readonly ServicesEndpointsConfig _endpointsConfig;

        public RemoveOfferCommandHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository,
            IGrpcServiceClientFactory<IOrdersService> ordersServiceClientFactory,
            IOptions<ServicesEndpointsConfig> options)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _ordersServiceClientFactory = ordersServiceClientFactory ?? throw new ArgumentNullException(nameof(ordersServiceClientFactory));
            _endpointsConfig = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public async Task<Unit> Handle(RemoveOfferCommand request, CancellationToken cancellationToken)
        {
            var offerId = Guid.Parse(request.OfferId);
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null) throw new NotFoundException();

            var userClaims = _httpContext.User.Claims.ToTokenPayload().UserClaims;
            var userId = userClaims.Id;
            var adminRoles = new List<string> { "ADMIN", "SUPER_ADMIN" };
            if (offer.OwnerId != userId && !adminRoles.Contains(userClaims.Role)) throw new ForbiddenException();

            var ordersServiceClient = _ordersServiceClientFactory.Create(_endpointsConfig.Orders.Grpc.ToString());
            var grpcRequest = new GetOfferHasOrdersRequest { OfferId = offerId };
            var grpcResponse = await ordersServiceClient.GetOfferHasOrders(grpcRequest);
            if (grpcResponse.OfferHasOrders)
                throw new OffersDomainException("Offer cannot be cancelled because it's related to at least one order");

            offer.MarkAsRemoved();

            _offerRepository.Update(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
