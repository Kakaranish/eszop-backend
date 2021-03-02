using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Common.Extensions;
using Common.Grpc;
using Common.Grpc.Services.OrdersService;
using Common.Grpc.Services.OrdersService.CreateOrder;
using Common.Logging;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.API.Application.Commands.FinalizeCart
{
    public class FinalizeCartCommandHandler : IRequestHandler<FinalizeCartCommand, Guid>
    {
        private readonly ILogger<FinalizeCartCommandHandler> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGrpcServiceClientFactory<IOrdersService> _ordersServiceClientFactory;
        private readonly HttpContext _httpContext;
        private readonly ServicesEndpointsConfig _endpointsConfig;

        public FinalizeCartCommandHandler(ILogger<FinalizeCartCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository,
            IHttpClientFactory httpClientFactory,
            IGrpcServiceClientFactory<IOrdersService> ordersServiceClientFactory,
            IOptions<ServicesEndpointsConfig> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _ordersServiceClientFactory = ordersServiceClientFactory ??
                                          throw new ArgumentNullException(nameof(ordersServiceClientFactory));
            _endpointsConfig = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public async Task<Guid> Handle(FinalizeCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);
            if (cart.IsEmpty)
                throw new CartsDomainException($"Cart {cart.Id} cannot be finalized because it's empty");

            var ordersServiceClient = _ordersServiceClientFactory.Create(_endpointsConfig.Orders.Grpc.ToString());
            var createOrderRequest = new CreateOrderRequest
            {
                Id = cart.Id,
                SellerId = cart.CartItems.First().SellerId,
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(x => new CartItemDto
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
                })
            };

            var createOrderResponse = await ordersServiceClient.CreateOrder(createOrderRequest);

            _logger.LogWithProps(LogLevel.Debug, "Order created from cart",
                "OrderId".ToKvp(createOrderResponse.OrderId),
                "CartId".ToKvp(cart.Id));

            _cartRepository.Remove(cart);
            await _cartRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            _logger.LogWithProps(LogLevel.Debug, "Removed cart", "CartId".ToKvp(cart.Id));

            return createOrderResponse.OrderId;
        }
    }
}
