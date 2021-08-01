using Common.Domain.Repositories;
using Identity.Domain.Aggregates.RefreshTokenAggregate;
using Identity.Domain.Aggregates.UserAggregate;
using System;
using System.Threading.Tasks;

namespace Identity.Domain.Repositories
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
