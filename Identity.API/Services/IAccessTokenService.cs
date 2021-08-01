using Common.Utilities.Authentication;

namespace Identity.API.Services
{
    public interface IAccessTokenService
    {
        string Create(UserClaims userClaims);
    }
}
