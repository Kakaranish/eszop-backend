using Common.DataAccess;
using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Identity.API.DataAccess.Repositories
{
    public class ProfileInfoRepository : IProfileInfoRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public ProfileInfoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<ProfileInfo> GetByUserIdAsync(Guid userId)
        {
            return await _appDbContext.ProfileInfos.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public void Add(ProfileInfo profileInfo)
        {
            _appDbContext.ProfileInfos.Add(profileInfo);
        }

        public void Update(ProfileInfo profileInfo)
        {
            _appDbContext.Update(profileInfo);
        }
    }
}
