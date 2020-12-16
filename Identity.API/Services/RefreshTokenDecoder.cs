using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Common.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Services
{
    public class RefreshTokenDecoder : IRefreshTokenDecoder
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly TokenValidationParameters _tokenValidationParams;

        public RefreshTokenDecoder(IOptions<JwtConfig> jwtOptions)
        {
            if (jwtOptions?.Value == null) throw new ArgumentNullException(nameof(jwtOptions));

            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _tokenValidationParams = CreateValidationParams(jwtOptions.Value);
        }

        private static TokenValidationParameters CreateValidationParams(JwtConfig jwtConfig)
        {
            var signingKeyAsBytes = Encoding.UTF8.GetBytes(jwtConfig.RefreshTokenSecretKey);
            var securityKey = new SymmetricSecurityKey(signingKeyAsBytes);

            return new TokenValidationParameters
            {
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidAlgorithms = new List<string> { SecurityAlgorithms.HmacSha256 },
                ClockSkew = TimeSpan.Zero
            };
        }

        public TokenPayload Decode(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            try
            {
                _jwtSecurityTokenHandler.ValidateToken(token, _tokenValidationParams, out var securityToken);
                if (!(securityToken is JwtSecurityToken jwtSecurityToken))
                {
                    return null;
                }

                var claimsDict = jwtSecurityToken.Claims.ToDictionary(x => x.Type, x => x.Value);

                return new TokenPayload
                {
                    Id = Guid.Parse(claimsDict["TokenId"]),
                    Issuer = jwtSecurityToken.Issuer,
                    Audience = jwtSecurityToken.Audiences.FirstOrDefault(),
                    Expires = jwtSecurityToken.ValidTo,
                    UserClaims = new UserClaims
                    {
                        Id = Guid.Parse(claimsDict["UserId"]),
                        Role = claimsDict["Role"],
                        Email = claimsDict["Email"]
                    }
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
