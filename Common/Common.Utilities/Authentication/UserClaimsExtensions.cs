using System.Collections.Generic;
using System.Security.Claims;

namespace Common.Utilities.Authentication
{
    public static class UserClaimsExtensions
    {
        public static IEnumerable<Claim> ToTokenClaims(this UserClaims userClaims)
        {
            return new List<Claim>
            {
                new Claim("UserId", userClaims.Id.ToString()),
                new Claim("Email", userClaims.Email),
                new Claim("Role", userClaims.Role)
            };
        }
    }
}
