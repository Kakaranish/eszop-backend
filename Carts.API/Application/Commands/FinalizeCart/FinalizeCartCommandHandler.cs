using Carts.Domain.Aggregates.CartAggregate;
using Carts.Domain.Aggregates.Repositories;
using Carts.Domain.Exceptions;
using Common.Grpc;
using Common.Grpc.Services.OffersService;
using Common.Grpc.Services.OffersService.Requests.GetOffersAvailability;
using Common.Grpc.Services.OrdersService;
using Common.Grpc.Services.OrdersService.CreateOrder;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Logging;
using Common.Utilities.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.API.Application.Commands.FinalizeCart
{
    public class FinalizeCartCommandHandler : IRequestHandler<FinalizeCartCommand, Guid>
    {
        private readonly ILogger<FinalizeCartCommandHandler> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly IGrpcServiceClientFactory<IOrdersService> _ordersServiceClientFactory;
        private readonly IGrpcServiceClientFactory<IOffersService> _offersServiceClientFactory;
        private readonly IEventBus _eventBus;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly HttpContext _httpContext;
        private readonly ServicesEndpointsConfig _endpointsConfig;

        public FinalizeCartCommandHandler(ILogger<FinalizeCartCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository,
            IGrpcServiceClientFactory<IOrdersService> ordersServiceClientFactory,
            IGrpcServiceClientFactory<IOffersService> offersServiceClientFactory,
            IOptions<ServicesEndpointsConfig> options, IEventBus eventBus,
            ICartItemRepository cartItemRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _ordersServiceClientFactory = ordersServiceClientFactory ??
                                          throw new ArgumentNullException(nameof(ordersServiceClientFactory));
            _offersServiceClientFactory = offersServiceClientFactory ?? throw new ArgumentNullException(nameof(offersServiceClientFactory));
            _endpointsConfig = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _cartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
        }

        public async Task<Guid> Handle(FinalizeCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);
            if (cart.IsEmpty)
                throw new CartsDomainException($"Cart {cart.Id} cannot be finalized because it's empty");

            var offersServiceClient = _offersServiceClientFactory.Create(_endpointsConfig.Offers.Grpc.ToString());
            var offerIds = cart.CartItems.Select(x => x.OfferId).ToList();
            var getOffersAvailabilityRequest = new GetOffersAvailabilityRequest { OfferIds = offerIds };
            var getOffersAvailabilityResponse = await offersServiceClient.GetOffersAvailability(getOffersAvailabilityRequest);

            var notActiveOffers = getOffersAvailabilityResponse.OfferAvailabilities.Where(x => x.Availability != Availability.Active).ToList();
            if (notActiveOffers.Any())
            {
                foreach (var offer in notActiveOffers.Where(x => x.Availability == Availability.NotActive))
                {
                    await _eventBus.PublishAsync(new OfferBecameUnavailableIntegrationEvent
                    {
                        OfferId = offer.OfferId
                    });

                    _logger.LogWithProps(LogLevel.Error,
                        $"Published {nameof(OfferBecameUnavailableIntegrationEvent)} integration event",
                        "CartId".ToKvp(cart.Id),
                        "OfferId".ToKvp(offer.OfferId));
                }

                var notExistingOffers = notActiveOffers.Where(x => x.Availability == Availability.DoesNotExist).ToList();
                if (notExistingOffers.Any())
                {
                    foreach (var notExistingOffer in notExistingOffers)
                    {
                        _cartItemRepository.RemoveWithOfferId(notExistingOffer.OfferId);
                    }

                    await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

                    _logger.LogWithProps(LogLevel.Error,
                        $"Removed not existing offers from cart",
                        "CartId".ToKvp(cart.Id),
                        "OffersIds".ToKvp(string.Join(",", notExistingOffers.Select(x => x.OfferId))));
                }

                throw new CartsDomainException("At least offer is not active");
            }

            var ordersServiceClient = _ordersServiceClientFactory.Create(_endpointsConfig.Orders.Grpc.ToString());
            var createOrderRequest = PrepareCreateOrderRequest(cart);
            var createOrderResponse = await ordersServiceClient.CreateOrder(createOrderRequest);

            _logger.LogWithProps(LogLevel.Debug, "Order created from cart",
                "OrderId".ToKvp(createOrderResponse.OrderId),
                "CartId".ToKvp(cart.Id));

            _cartRepository.Remove(cart);
            await _cartRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            _logger.LogWithProps(LogLevel.Debug, "Removed cart", "CartId".ToKvp(cart.Id));

            return createOrderResponse.OrderId;
        }

        private static CreateOrderRequest PrepareCreateOrderRequest(Cart cart)
        {
            return new()
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
        }
    }
}
