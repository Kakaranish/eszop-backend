using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly SigningCredentials _tokenSigningCredentials;

        public AccessTokenService(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig?.Value ?? throw new ArgumentNullException(nameof(jwtConfig));
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _tokenSigningCredentials = CreateTokenSigningCredentials();
        }

        private SigningCredentials CreateTokenSigningCredentials()
        {
            var secretToken = Encoding.UTF8.GetBytes(_jwtConfig.AccessTokenSecretKey);
            var securityKey = new SymmetricSecurityKey(secretToken);

            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        public string Create(UserClaims userClaims)
        {
            if (userClaims == null) throw new ArgumentNullException(nameof(userClaims));

            var tokenId = Guid.NewGuid();
            var claims = new List<Claim> { new Claim("TokenId", tokenId.ToString()) };
            claims.AddRange(userClaims.ToTokenClaims());

            var jwtHeader = new JwtHeader(_tokenSigningCredentials);
            var jwtPayload = new JwtPayload(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                notBefore: null,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpirationInMinutes));

            var jwtToken = new JwtSecurityToken(jwtHeader, jwtPayload);
            var tokenStr = _jwtSecurityTokenHandler.WriteToken(jwtToken);

            return tokenStr;
        }
    }
}
