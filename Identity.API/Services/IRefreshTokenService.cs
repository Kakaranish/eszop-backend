using System.Threading.Tasks;
using Identity.Domain.Aggregates;
using Identity.Domain.Aggregates.RefreshTokenAggregate;
using Identity.Domain.Aggregates.UserAggregate;

namespace Identity.API.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GetOrCreateAsync(User user);
        Task<RefreshToken> CreateAsync(User user);
        Task<bool> VerifyAsync(string refreshTokenPayload);
        Task<bool> TryRevokeAsync(string refreshTokenPayload);
        Task<string> RefreshAccessToken(string refreshTokenStr);
    }
}
