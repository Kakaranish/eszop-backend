using Common.Authentication;
using Common.Types;
using Identity.API.Services;
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

        public TokenController(IAccessTokenDecoder accessTokenDecoder, IRefreshTokenDecoder refreshTokenDecoder,
            IRefreshTokenService refreshTokenService)
        {
            _accessTokenDecoder = accessTokenDecoder ?? throw new ArgumentNullException(nameof(accessTokenDecoder));
            _refreshTokenDecoder = refreshTokenDecoder ?? throw new ArgumentNullException(nameof(refreshTokenDecoder));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
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
        public async Task<IActionResult> RefreshAccessToken([FromBody] string refreshToken)
        {
            var result = await _refreshTokenService.RefreshAccessToken(refreshToken);

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
