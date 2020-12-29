using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace Carts.API.Application.Commands.RemoveFromCart
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand>
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly HttpContext _httpContext;

        public RemoveFromCartCommandHandler(ICartItemRepository cartItemRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
        }

        public async Task<Unit> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var cartItemId = Guid.Parse(request.CartItemId);
            var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;

            if (userId != cartItem.SellerId)
            {
                throw new CartsDomainException($"Cart item {cartItemId} not found");
            }

            _cartItemRepository.Remove(cartItem);
            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
