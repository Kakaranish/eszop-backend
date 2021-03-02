using Carts.API.Application.Dto;
using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Carts.API.Extensions;
using Common.Extensions;
using Common.Grpc;
using Common.Grpc.Services.OffersService;
using Common.Grpc.Services.OffersService.Requests.GetOfferBasicInfo;
using Common.Logging;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.API.Application.Commands.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, CartItemDto>
    {
        private readonly ILogger<AddToCartCommandHandler> _logger;
        private readonly HttpContext _httpContext;
        private readonly ICartRepository _cartRepository;
        private readonly IGrpcServiceClientFactory<IOffersService> _offersServiceClientFactory;
        private readonly ServicesEndpointsConfig _endpointsConfig;

        public AddToCartCommandHandler(ILogger<AddToCartCommandHandler> logger, IHttpContextAccessor httpContextAccessor,
            ICartRepository cartRepository, IGrpcServiceClientFactory<IOffersService> offersServiceClientFactory,
            IOptions<ServicesEndpointsConfig> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _offersServiceClientFactory = offersServiceClientFactory ??
                                          throw new ArgumentNullException(nameof(offersServiceClientFactory));
            _endpointsConfig = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public async Task<CartItemDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var offerId = Guid.Parse(request.OfferId);
            var offersServiceClient = _offersServiceClientFactory.Create(_endpointsConfig.Offers.Grpc.ToString());
            var grpcRequest = new GetOfferBasicInfoRequest { OfferId = offerId };
            var grpcResponse = await offersServiceClient.GetOfferBasicInfo(grpcRequest);
            var offer = grpcResponse.Offer;

            if (request.Quantity > offer.AvailableStock)
                throw new CartsDomainException($"Quantity out of range. AvailableStock for offer {offer.Id} is {offer.AvailableStock}");

            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            if (userId == offer.OwnerId)
                throw new CartsDomainException("Buying from himself/herself is illegal");

            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);

            var imageUri = offer.Images.FirstOrDefault(x => x.IsMain)?.Uri;
            var cartItem = new CartItem(cart.Id, Guid.Parse(request.OfferId), offer.OwnerId,
                offer.Name, offer.Price, request.Quantity, offer.AvailableStock, imageUri);
            cart.AddCartItem(cartItem);

            _cartRepository.Update(cart);
            await _cartRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            _logger.LogWithProps(LogLevel.Debug, "Offer added to cart",
                "OfferId".ToKvp(request.OfferId),
                "CartId".ToKvp(cart.Id),
                "CartItemId".ToKvp(cartItem.Id));

            return cartItem.ToDto();
        }
    }
}
