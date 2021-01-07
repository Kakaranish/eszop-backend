using Identity.API.Application.Dto;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using Identity.API.Extensions;
using Identity.API.Services;
using MediatR;
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

        public SignUpCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher,
            IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
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

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
