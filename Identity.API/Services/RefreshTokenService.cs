using Common.Authentication;
using Common.Extensions;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using Identity.API.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenDecoder _refreshTokenDecoder;
        private readonly IAccessTokenService _accessTokenService;
        private readonly ILogger<RefreshTokenService> _logger;
        private readonly JwtConfig _jwtConfig;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly SigningCredentials _tokenSigningCredentials;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IRefreshTokenDecoder refreshTokenDecoder,
            IAccessTokenService accessTokenService, IOptions<JwtConfig> jwtConfig, ILogger<RefreshTokenService> logger)
        {
            _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
            _refreshTokenDecoder = refreshTokenDecoder ?? throw new ArgumentNullException(nameof(refreshTokenDecoder));
            _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
            _jwtConfig = jwtConfig?.Value ?? throw new ArgumentNullException(nameof(jwtConfig));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _tokenSigningCredentials = CreateTokenSigningCredentials();
        }

        private SigningCredentials CreateTokenSigningCredentials()
        {
            var signingKeyAsBytes = Encoding.UTF8.GetBytes(_jwtConfig.RefreshTokenSecretKey);
            var securityKey = new SymmetricSecurityKey(signingKeyAsBytes);

            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        public async Task<RefreshToken> CreateAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await CreateInternalAsync(user);
        }

        public async Task<bool> VerifyAsync(string refreshTokenPayload)
        {
            if (refreshTokenPayload == null) return false;

            var refreshToken = await _refreshTokenRepository.GetByPayloadAsync(refreshTokenPayload);

            return refreshToken != null && refreshToken.IsRevoked == false;
        }

        public async Task<RefreshToken> GetOrCreateAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var existingToken = await _refreshTokenRepository.GetActiveByUserAsync(user);

            return existingToken ?? await CreateInternalAsync(user);
        }

        public async Task<bool> TryRevokeAsync(string refreshTokenPayload)
        {
            if (refreshTokenPayload == null) throw new ArgumentNullException(nameof(refreshTokenPayload));

            var refreshToken = await _refreshTokenRepository.GetByPayloadAsync(refreshTokenPayload);
            if (refreshToken == null || refreshToken.IsRevoked)
            {
                return false;
            }

            refreshToken.Revoke();

            _refreshTokenRepository.Update(refreshToken);
            await _refreshTokenRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            return true;
        }

        public async Task<string> RefreshAccessToken(string refreshTokenStr)
        {
            var decodedRefreshToken = _refreshTokenDecoder.Decode(refreshTokenStr);
            if (decodedRefreshToken == null)
            {
                _logger.LogWarning("Cannot refresh access token - decoding failure");
                return null;
            }

            var refreshToken = await _refreshTokenRepository.GetByIdAsync(decodedRefreshToken.Id);
            if (refreshToken?.IsRevoked ?? false)
            {
                _logger.LogWarning("Cannot refresh access token - no such refresh token or token is revoked");
                return null;
            }

            return _accessTokenService.Create(decodedRefreshToken.UserClaims);
        }

        private async Task<RefreshToken> CreateInternalAsync(User user)
        {
            var tokenId = Guid.NewGuid();
            var userClaims = user.ExtractUserClaims();
            var jwtToken = CreateJwtToken(userClaims, tokenId);
            var jwtTokenStr = _jwtSecurityTokenHandler.WriteToken(jwtToken);

            var refreshToken = new RefreshToken(user.Id, jwtTokenStr);
            refreshToken = refreshToken.Bind(x => x.Id, tokenId);

            _refreshTokenRepository.Add(refreshToken);
            await _refreshTokenRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            return refreshToken;
        }

        private JwtSecurityToken CreateJwtToken(UserClaims userClaims, Guid tokenId)
        {
            var claims = new List<Claim> { new Claim("TokenId", tokenId.ToString()) };
            claims.AddRange(userClaims.ToTokenClaims());

            var jwtPayload = new JwtPayload(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                notBefore: null,
                expires: DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpirationInDays));

            var jwtHeader = new JwtHeader(_tokenSigningCredentials);
            return new JwtSecurityToken(jwtHeader, jwtPayload);
        }
    }
}
