using Carts.API.DataAccess.Repositories;
using Common.Extensions;
using Common.IntegrationEvents;
using Common.IntegrationEvents.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.EventBus;

namespace Carts.API.Application.Commands.FinalizeCart
{
    public class FinalizeCartCommandHandler : IRequestHandler<FinalizeCartCommand>
    {
        private readonly ILogger<FinalizeCartCommandHandler> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly IEventBus _eventBus;
        private readonly HttpContext _httpContext;

        public FinalizeCartCommandHandler(ILogger<FinalizeCartCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<Unit> Handle(FinalizeCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);
            if (cart.IsEmpty)
            {
                var msg = $"Cart {cart.Id} cannot be finalized because it's empty";
                _logger.LogError(msg);
                throw new CartDomainException(msg);
            }

            var @event = new CartFinalizedIntegrationEvent
            {
                UserId = userId,
                CartItems = cart.CartItems.Select(item =>
                    new CartItemDto
                    {
                        OfferId = item.OfferId,
                        PricePerItem = item.PricePerItem,
                        Quantity = item.Quantity,
                        OfferName = item.OfferName
                    }).ToList()
            };
            await _eventBus.PublishAsync(@event);

            return await Unit.Task;
        }
    }
}
