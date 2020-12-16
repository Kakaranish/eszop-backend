using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using Identity.API.Dto;
using Identity.API.Extensions;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Common.Types;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AuthController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthController(IUserRepository userRepository, IAccessTokenService accessTokenService,
            IRefreshTokenService refreshTokenService, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        [HttpPost("sign-in")]
        [ProducesResponseType(200, Type = typeof(TokenResponse))]
        public async Task<IActionResult> SignIn(SignInDto signInDto)
        {
            var user = await _userRepository.FindByEmailAsync(signInDto.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var isPasswordValid = _passwordHasher.Verify(signInDto.Password, user.HashedPassword);
            if (!isPasswordValid)
            {
                return Unauthorized();
            }

            var userClaims = user.ExtractUserClaims();
            var accessToken = _accessTokenService.Create(userClaims);
            var refreshToken = await _refreshTokenService.GetOrCreateAsync(user);

            return Ok(new TokenResponse(accessToken, refreshToken.Token));
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            var otherUser = await _userRepository.FindByEmailAsync(signUpDto.Email);
            if (otherUser != null)
            {
                return ErrorResponse("Other user has the same email");
            }

            var password = _passwordHasher.Hash(signUpDto.Password);
            var user = new User(signUpDto.Email, password, Role.User, signUpDto.FirstName, signUpDto.LastName);
            await _userRepository.AddUserAsync(user);

            var userClaims = user.ExtractUserClaims();
            var accessToken = _accessTokenService.Create(userClaims);
            var refreshToken = await _refreshTokenService.GetOrCreateAsync(user);

            return Ok(new TokenResponse(accessToken, refreshToken.Token));
        }
    }
}
