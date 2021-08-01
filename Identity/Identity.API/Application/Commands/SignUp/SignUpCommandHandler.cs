using Common.Utilities.Authentication;
using Identity.API.Application.Dto;
using Identity.API.Configuration;
using Identity.API.Extensions;
using Identity.API.Services;
using Identity.Domain.Aggregates.UserAggregate;
using Identity.Domain.Exceptions;
using Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.SignUp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, TokenResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly HttpContext _httpContext;
        private readonly JwtConfig _jwtConfig;

        public SignUpCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher,
            IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService,
            IHttpContextAccessor httpContextAccessor, IOptions<JwtConfig> jwtConfigOptions)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _jwtConfig = jwtConfigOptions.Value ?? throw new ArgumentNullException(nameof(jwtConfigOptions.Value));
        }

        public async Task<TokenResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var otherUser = await _userRepository.GetByEmailAsync(request.Email);
            if (otherUser != null) throw new IdentityDomainException("Other user has the same email");

            var password = _passwordHasher.Hash(request.Password);
            var user = new User(request.Email, password, Role.User);
            user.SetLastLoginToNow();
            _userRepository.AddUser(user);

            await _userRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            var userClaims = user.ExtractUserClaims();
            var accessToken = _accessTokenService.Create(userClaims);
            var refreshToken = await _refreshTokenService.GetOrCreateAsync(user);

            _httpContext.Response.Cookies.Append("accessToken", accessToken, CookieSettings.PrivateCookie);
            _httpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, CookieSettings.PrivateCookie);

            var accessTokenExp = DateTimeOffset.UtcNow
                .AddMinutes(_jwtConfig.AccessTokenExpirationInMinutes).ToUnixTimeSeconds();
            _httpContext.Response.Cookies.Append("accessTokenExp", $"{accessTokenExp}", CookieSettings.PublicCookie);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
