using Carts.API.Application.Dto;
using Carts.API.DataAccess.Repositories;
using Carts.API.Extensions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.API.Application.Queries.GetOrCreateCart
{
    public class GetOrCreateCartQueryHandler : IRequestHandler<GetOrCreateCartQuery, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly HttpContext _httpContext;

        public GetOrCreateCartQueryHandler(IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository)
        {
            _httpContext = httpContextAccessor?.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public async Task<CartDto> Handle(GetOrCreateCartQuery request, CancellationToken cancellationToken)
        {
            var tokenPayload = _httpContext.User.Claims.ToTokenPayload();
            var userId = tokenPayload.UserClaims.Id;
            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);

            return cart.ToDto();
        }
    }
}