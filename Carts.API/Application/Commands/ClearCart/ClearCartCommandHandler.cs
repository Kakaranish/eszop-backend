using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Carts.API.DataAccess.Repositories;
using Common.Extensions;

namespace Carts.API.Application.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly HttpContext _httpContext;

        public ClearCartCommandHandler(IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public async Task<Unit> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);
            cart.ClearCartItems();
            
            _cartRepository.UpdateAsync(cart);
            await _cartRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
