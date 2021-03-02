using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Common.Dto;
using Common.Extensions;
using Common.Logging;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.API.Application.Commands.FinalizeCart
{
    public class FinalizeCartCommandHandler : IRequestHandler<FinalizeCartCommand, Guid>
    {
        private readonly ILogger<FinalizeCartCommandHandler> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpContext _httpContext;
        private readonly ServicesEndpointsConfig _servicesEndpointsConfig;

        public FinalizeCartCommandHandler(ILogger<FinalizeCartCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository,
            IHttpClientFactory httpClientFactory, IOptions<ServicesEndpointsConfig> servicesEndpointsConfigOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _servicesEndpointsConfig = servicesEndpointsConfigOptions.Value ??
                                       throw new ArgumentNullException(nameof(servicesEndpointsConfigOptions.Value));
        }

        public async Task<Guid> Handle(FinalizeCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);
            if (cart.IsEmpty)
                throw new CartsDomainException($"Cart {cart.Id} cannot be finalized because it's empty");

            var httpClient = _httpClientFactory.CreateClient();
            var accessToken = _httpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var uri = $"{_servicesEndpointsConfig.Orders.Api}/api/orders"; // TODO:
            var content = new StringContent(JsonConvert.SerializeObject(ToCreateOrderCartDto(cart)), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri, content, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var orderCreatedDto = JsonConvert.DeserializeObject<OrderCreatedDto>(responseContent);

            _logger.LogWithProps(LogLevel.Debug, "Order created from cart",
                "OrderId".ToKvp(orderCreatedDto.OrderId),
                "CartId".ToKvp(cart.Id));

            _cartRepository.Remove(cart);
            await _cartRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            _logger.LogWithProps(LogLevel.Debug, "Removed cart", "CartId".ToKvp(cart.Id));

            return orderCreatedDto.OrderId;
        }

        private static CreateOrderCartDto ToCreateOrderCartDto(Cart cart)
        {
            return new()
            {
                Id = cart.Id,
                UserId = cart.UserId,
                SellerId = cart.CartItems.First().SellerId,
                CartItems = cart.CartItems.Select(x => new CreateOrderCartItemDto
                {
                    Id = x.Id,
                    CartId = x.CartId,
                    SellerId = x.SellerId,
                    OfferId = x.OfferId,
                    OfferName = x.OfferName,
                    AvailableStock = x.AvailableStock,
                    Quantity = x.Quantity,
                    PricePerItem = x.PricePerItem,
                    ImageUri = x.ImageUri
                }).ToList()
            };
        }
    }
}
