using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Common.Extensions;
using Common.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.API.Application.Commands.RemoveFromCart
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand>
    {
        private readonly ILogger<RemoveFromCartCommandHandler> _logger;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly HttpContext _httpContext;

        public RemoveFromCartCommandHandler(ILogger<RemoveFromCartCommandHandler> logger,
            ICartItemRepository cartItemRepository, IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public async Task<Unit> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var cartItemId = Guid.Parse(request.CartItemId);
            var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);

            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);
            if (cartItem == null || cartItem.CartId != cart.Id)
                throw new CartsDomainException($"Cart item {cartItemId} not found");

            _cartItemRepository.Remove(cartItem);
            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            _logger.LogWithProps(LogLevel.Debug, "Removed cart item",
                "CartId".ToKvp(cartItem.CartId),
                "OfferId".ToKvp(cartItem.OfferId),
                "CartItemId".ToKvp(cartItem.Id));

            return await Unit.Task;
        }
    }
}
