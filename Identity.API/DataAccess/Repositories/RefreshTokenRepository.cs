using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Identity.API.DataAccess.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _appDbContext;
        
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

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _appDbContext.RefreshTokens.AddAsync(refreshToken);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _appDbContext.Update(refreshToken);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
