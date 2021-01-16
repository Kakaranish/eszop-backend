using Common.Exceptions;
using Identity.API.Application.Dto;
using Identity.API.DataAccess.Repositories;
using Identity.API.Extensions;
using Identity.API.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
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

        public SignInCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher,
            IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
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

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.MaxValue
            };
            _httpContext.Response.Cookies.Append("accessToken", accessToken, cookieOptions);
            _httpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
