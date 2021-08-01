using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Common.Domain.Repositories;

namespace Identity.API.DataAccess.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public RefreshTokenRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<RefreshToken> GetByIdAsync(Guid tokenId)
        {
            return await _appDbContext.RefreshTokens.FirstOrDefaultAsync(token => token.Id == tokenId);
        }

        public async Task<RefreshToken> GetByPayloadAsync(string refreshTokenPayload)
        {
            return await _appDbContext.RefreshTokens.FirstOrDefaultAsync(token =>
                token.Token == refreshTokenPayload);
        }

        public async Task<RefreshToken> GetActiveByUserAsync(User user)
        {
            return await _appDbContext.RefreshTokens.FirstOrDefaultAsync(token =>
                token.UserId == user.Id && token.RevokedAt == null);
        }

        public void Add(RefreshToken refreshToken)
        {
            _appDbContext.RefreshTokens.Add(refreshToken);
        }

        public void Update(RefreshToken refreshToken)
        {
            _appDbContext.Update(refreshToken);
        }
    }
}
