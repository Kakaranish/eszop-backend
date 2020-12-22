using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Common.Authentication;

namespace Common.Extensions
{
    public static class ClaimsExtensions
    {
        public static TokenPayload ToTokenPayload(this IEnumerable<Claim> claims)
        {
            var claimsDict = claims.ToDictionary(x => x.Type, x => x.Value);
            
            return new TokenPayload
            {
                Id = Guid.Parse(claimsDict["TokenId"]),
                Issuer = claimsDict["iss"],
                Audience = claimsDict["aud"],
                Expires = UnixTimeStampToDateTime(claimsDict["exp"]),
                UserClaims = new UserClaims
                {
                    Id = Guid.Parse(claimsDict["UserId"]),
                    Role = claimsDict["Role"],
                    Email = claimsDict["Email"]
                }
            };
        }

        private static DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dtDateTime.AddSeconds(long.Parse(unixTimeStamp)).ToLocalTime();
        }
    }
}
