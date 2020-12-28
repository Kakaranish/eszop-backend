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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.API.Application.Commands.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand>
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

        public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var uri = $"https://{_urlsConfig.Offers}/api/offers/{request.OfferId}";
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(uri, cancellationToken);
            var contentStr = await response.Content.ReadAsStringAsync(cancellationToken);
            var offerDto = JsonConvert.DeserializeObject<OfferDto>(contentStr);

            // TODO: Validate if offer does not belong to user
            
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);

            // TODO: Validate if offer is not already in cart
            // TODO: Validate if offer quantity is legal
            // TODO: Validate if there is no offer from other seller in cart

            var cartItem = new CartItem(cart.Id, Guid.Parse(request.OfferId), userId, 
                offerDto.Name, request.Quantity, offerDto.Price);

            cart.AddCartItem(cartItem);

            await _cartRepository.UpdateAsync(cart);

            return await Unit.Task;
        }

        private class OfferDto
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
    }
}
