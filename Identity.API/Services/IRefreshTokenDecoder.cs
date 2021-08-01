using Common.Utilities.Authentication;

namespace Identity.API.Services
{
    public interface IRefreshTokenDecoder
    {
        TokenPayload Decode(string refreshToken);
    }
}
