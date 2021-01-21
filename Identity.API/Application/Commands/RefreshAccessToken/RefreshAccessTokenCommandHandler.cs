using Identity.API.Domain;
using Identity.API.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.RefreshAccessToken
{
    public class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, string>
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly HttpContext _httpContext;

        public RefreshAccessTokenCommandHandler(IHttpContextAccessor httpContextAccessor,
            IRefreshTokenService refreshTokenService)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
        }

        public async Task<string> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            if (!_httpContext.Request.Headers.TryGetValue("refreshToken", out var refreshToken))
                throw new IdentityDomainException("No refresh token provided");

            var accessToken = await _refreshTokenService.RefreshAccessToken(refreshToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.MaxValue
            };
            _httpContext.Response.Cookies.Append("accessToken", accessToken, cookieOptions);

            return accessToken;
        }
    }
}
