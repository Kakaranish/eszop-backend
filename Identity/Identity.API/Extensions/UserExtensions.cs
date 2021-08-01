using Common.Utilities.Authentication;
using Identity.Domain.Aggregates.UserAggregate;

namespace Identity.API.Extensions
{
    public static class UserExtensions
    {
        public static UserClaims ExtractUserClaims(this User user)
        {
            return new()
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.Name
            };
        }
    }
}
