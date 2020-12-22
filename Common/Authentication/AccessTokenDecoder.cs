using Common.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Common.Authentication
{
    public class AccessTokenDecoder : IAccessTokenDecoder
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly TokenValidationParameters _tokenValidationParams;

        public AccessTokenDecoder(IOptions<JwtConfig> jwtOptions)
        {
            if (jwtOptions?.Value == null) throw new ArgumentNullException(nameof(jwtOptions));

            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _tokenValidationParams = CreateTokenValidationParams(jwtOptions.Value);
        }

        private static TokenValidationParameters CreateTokenValidationParams(JwtConfig jwtConfig)
        {
            var signingKeyAsBytes = Encoding.UTF8.GetBytes(jwtConfig.AccessTokenSecretKey);

            return new TokenValidationParameters
            {
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(signingKeyAsBytes),
                ValidAlgorithms = new List<string> { SecurityAlgorithms.HmacSha256 }
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

                return jwtSecurityToken.Claims.ToTokenPayload();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
