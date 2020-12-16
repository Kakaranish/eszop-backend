using Common.Authentication;

namespace Identity.API.Services
{
    public interface IAccessTokenService
    {
        string Create(UserClaims userClaims);
    }
}
