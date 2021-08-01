using Common.Utilities.Authentication;
using Common.Utilities.Types;
using Identity.API.Application.Commands.RefreshAccessToken;
using Identity.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class TokenController : BaseController
    {
        private readonly IAccessTokenDecoder _accessTokenDecoder;
        private readonly IRefreshTokenDecoder _refreshTokenDecoder;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMediator _mediator;

        public TokenController(IAccessTokenDecoder accessTokenDecoder, IRefreshTokenDecoder refreshTokenDecoder,
            IRefreshTokenService refreshTokenService, IMediator mediator)
        {
            _accessTokenDecoder = accessTokenDecoder ?? throw new ArgumentNullException(nameof(accessTokenDecoder));
            _refreshTokenDecoder = refreshTokenDecoder ?? throw new ArgumentNullException(nameof(refreshTokenDecoder));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("decode/access-token")]
        public TokenPayload DecodeAccessToken([FromBody] string accessToken)
        {
            return _accessTokenDecoder.Decode(accessToken);
        }

        [HttpPost("decode/refresh-token")]
        public TokenPayload DecodeRefreshToken([FromBody] string refreshToken)
        {
            return _refreshTokenDecoder.Decode(refreshToken);
        }

        [HttpPost("verify/refresh-token")]
        public async Task<IActionResult> VerifyRefreshToken([FromBody] string refreshToken)
        {
            var isValid = await _refreshTokenService.VerifyAsync(refreshToken);

            return isValid
                ? Ok(_refreshTokenDecoder.Decode(refreshToken))
                : ErrorResponse("Invalid/expired/revoked token");
        }

        [HttpPost("refresh/access-token")]
        public async Task<IActionResult> RefreshAccessTokenFromHeader()
        {
            var command = new RefreshAccessTokenCommand();
            var result = await _mediator.Send(command);

            return result != null
                ? Ok(result)
                : ErrorResponse("Invalid/expired/revoked refresh token");
        }

        [HttpPost("revoke/refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] string refreshToken)
        {
            var revoked = await _refreshTokenService.TryRevokeAsync(refreshToken);

            return revoked
                ? Ok()
                : ErrorResponse("Invalid/expired/revoked refresh token");
        }
    }
}
