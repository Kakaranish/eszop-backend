using Identity.API.Domain;
using System;
using System.Threading.Tasks;
using Common.Domain.Repositories;

namespace Identity.API.DataAccess.Repositories
{
    public interface IRefreshTokenRepository : IDomainRepository<RefreshToken>
    {
        Task<RefreshToken> GetByIdAsync(Guid tokenId);
        Task<RefreshToken> GetByPayloadAsync(string refreshTokenPayload);
        Task<RefreshToken> GetActiveByUserAsync(User user);
        void Add(RefreshToken refreshToken);
        void Update(RefreshToken refreshToken);
    }
}
