using Carts.API.Application.Dto;
using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Common.Extensions;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.API.Application.Commands.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Guid>
    {
        private readonly ILogger<AddToCartCommandHandler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICartRepository _cartRepository;
        private readonly UrlsConfig _urlsConfig;
        private readonly HttpContext _httpContext;

        public AddToCartCommandHandler(ILogger<AddToCartCommandHandler> logger, IOptions<UrlsConfig> options,
            IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor,
            ICartRepository cartRepository)
        {
            _urlsConfig = options?.Value ?? throw new ArgumentNullException(nameof(options.Value));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public async Task<Guid> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var uri = $"{_urlsConfig.Offers}/api/offers/{request.OfferId}";
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(uri, cancellationToken);
            var contentStr = await response.Content.ReadAsStringAsync(cancellationToken);
            var offerDto = JsonConvert.DeserializeObject<OfferDto>(contentStr);

            if (request.Quantity > offerDto.AvailableStock)
            {
                throw new CartsDomainException($"Quantity out of range. AvailableStock for offer {offerDto.Id} is {offerDto.AvailableStock}");
            }

            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            //if (userId == offerDto.OwnerId)
            //{
            //    throw new CartsDomainException("Buying from himself/herself is illegal");
            //}

            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);

            var imageUri = offerDto.Images.FirstOrDefault(x => x.IsMain)?.Uri;
            var cartItem = new CartItem(cart.Id, Guid.Parse(request.OfferId), userId,
                offerDto.Name, offerDto.Price, request.Quantity, offerDto.AvailableStock, imageUri);
            cart.AddCartItem(cartItem);

            _cartRepository.Update(cart);
            await _cartRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            _logger.LogInformation($"Added {request.OfferId} offer to cart {cart.Id} as cart item {cartItem.Id}");

            return cartItem.Id;
        }
    }
}
