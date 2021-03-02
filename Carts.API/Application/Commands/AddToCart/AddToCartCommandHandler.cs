using Carts.API.Application.Dto;
using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Carts.API.Extensions;
using Carts.API.Grpc;
using Common.Extensions;
using Common.Grpc.Services.OffersService.Requests.GetOfferBasicInfo;
using Common.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.API.Application.Commands.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, CartItemDto>
    {
        private readonly ILogger<AddToCartCommandHandler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpContext _httpContext;
        private readonly ICartRepository _cartRepository;
        private readonly IOffersServiceClientFactory _offersServiceClientFactory;

        public AddToCartCommandHandler(ILogger<AddToCartCommandHandler> logger, IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository,
            IOffersServiceClientFactory offersServiceClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _offersServiceClientFactory = offersServiceClientFactory ??
                                          throw new ArgumentNullException(nameof(offersServiceClientFactory));
        }

        public async Task<CartItemDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var offerId = Guid.Parse(request.OfferId);
            var offersServiceClient = _offersServiceClientFactory.Create();
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
