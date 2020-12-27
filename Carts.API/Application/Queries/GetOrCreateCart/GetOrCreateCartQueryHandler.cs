using System;
using System.Threading;
using System.Threading.Tasks;
using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Carts.API.Application.Queries.GetOrCreateCart
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
