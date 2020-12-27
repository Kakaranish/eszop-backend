using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Carts.API.DataAccess.Repositories;
using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Carts.API.Application.Commands.FinalizeCart
{
    public class FinalizeCartCommandHandler : IRequestHandler<FinalizeCartCommand>
    {
        private readonly ILogger<FinalizeCartCommandHandler> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly HttpContext _httpContext;

        public FinalizeCartCommandHandler(ILogger<FinalizeCartCommandHandler> logger, 
            IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
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

            // TODO: Checkout logic here
            
            return await Unit.Task;
        }
    }
}
