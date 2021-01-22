using Common.Authentication;
using Identity.API.Configuration;
using Identity.API.Domain;
using Identity.API.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.RefreshAccessToken
{
    public class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, string>
    {
        private readonly HttpContext _httpContext;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtConfig _jwtConfig;

        public RefreshAccessTokenCommandHandler(IHttpContextAccessor httpContextAccessor,
            IRefreshTokenService refreshTokenService, IOptions<JwtConfig> jwtConfigOptions)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            _jwtConfig = jwtConfigOptions.Value ?? throw new ArgumentNullException(nameof(jwtConfigOptions.Value));
        }

        public async Task<string> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            if (!_httpContext.Request.Headers.TryGetValue("refreshToken", out var refreshToken))
                throw new IdentityDomainException("No refresh token provided");

            var accessToken = await _refreshTokenService.RefreshAccessToken(refreshToken);

            _httpContext.Response.Cookies.Delete("accessToken");
            _httpContext.Response.Cookies.Append("accessToken", accessToken, CookieSettings.PrivateCookie);

            _httpContext.Response.Cookies.Delete("accessTokenExp");
            var accessTokenExp = DateTimeOffset.UtcNow.AddMinutes(
                _jwtConfig.AccessTokenExpirationInMinutes).ToUnixTimeSeconds();
            _httpContext.Response.Cookies.Append(
                "accessTokenExp", $"{accessTokenExp}", CookieSettings.PublicCookie);

            return accessToken;
        }
    }
}
