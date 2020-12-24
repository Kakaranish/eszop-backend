using Carts.API.DataAccess.Repositories;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Carts.API.Domain;

namespace Carts.API.Application.Queries
{
    public class GetOrCreateCartQueryHandler : IRequestHandler<GetOrCreateCartQuery, Cart>
    {
        private readonly ICartRepository _cartRepository;
        private readonly HttpContext _httpContext;

        public GetOrCreateCartQueryHandler(IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository)
        {
            _httpContext = httpContextAccessor?.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public async Task<Cart> Handle(GetOrCreateCartQuery request, CancellationToken cancellationToken)
        {
            var tokenPayload = _httpContext.User.Claims.ToTokenPayload();
            var userId = tokenPayload.UserClaims.Id;

            return await _cartRepository.GetOrCreateByUserIdAsync(userId);
        }
    }
}
