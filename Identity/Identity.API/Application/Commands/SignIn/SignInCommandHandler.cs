using Common.Utilities.Authentication;
using Common.Utilities.Exceptions;
using Identity.API.Application.Dto;
using Identity.API.Configuration;
using Identity.API.Extensions;
using Identity.API.Services;
using Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, TokenResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly HttpContext _httpContext;
        private readonly JwtConfig _jwtConfig;

        public SignInCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher,
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

        public async Task<TokenResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null) throw new UnauthorizedException();

            var isPasswordValid = _passwordHasher.Verify(request.Password, user.HashedPassword);
            if (!isPasswordValid) throw new UnauthorizedException();

            if (user.IsLocked) throw new ForbiddenException();

            var userClaims = user.ExtractUserClaims();
            var accessToken = _accessTokenService.Create(userClaims);
            var refreshToken = await _refreshTokenService.GetOrCreateAsync(user);

            user.SetLastLoginToNow();

            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

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
