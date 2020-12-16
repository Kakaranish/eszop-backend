using Common.Types;
using Identity.API.Domain;
using System;
using System.Threading.Tasks;

namespace Identity.API.DataAccess.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetByIdAsync(Guid tokenId);
        Task<RefreshToken> GetByPayloadAsync(string refreshTokenPayload);
        Task<RefreshToken> GetActiveByUserAsync(User user);
        Task AddAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
    }
}
