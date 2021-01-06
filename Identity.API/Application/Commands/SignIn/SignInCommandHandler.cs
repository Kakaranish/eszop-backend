using Identity.API.Application.Dto;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using Identity.API.Extensions;
using Identity.API.Services;
using MediatR;
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

        public SignInCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher,
            IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
        }

        public async Task<TokenResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user == null) throw new UnauthorizedException();

            var isPasswordValid = _passwordHasher.Verify(request.Password, user.HashedPassword);
            if (!isPasswordValid) throw new UnauthorizedException();

            var userClaims = user.ExtractUserClaims();
            var accessToken = _accessTokenService.Create(userClaims);
            var refreshToken = await _refreshTokenService.GetOrCreateAsync(user);

            return new TokenResponse(accessToken, refreshToken.Token);
        }
    }
}
