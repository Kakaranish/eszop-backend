using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Carts.API.DataAccess.Repositories;
using Common.Extensions;
using Microsoft.Extensions.Logging;

namespace Carts.API.Application.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand>
    {
        private readonly ILogger<ClearCartCommandHandler> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly HttpContext _httpContext;

        public ClearCartCommandHandler(ILogger<ClearCartCommandHandler> logger, 
            IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public async Task<Unit> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);
            cart.ClearCartItems();
            
            _cartRepository.Update(cart);
            await _cartRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            _logger.LogInformation($"Cart {cart.Id} cleared");
            
            return await Unit.Task;
        }
    }
}
