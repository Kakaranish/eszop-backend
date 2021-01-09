using Common.DataAccess;
using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Identity.API.DataAccess.Repositories
{
    public class SellerInfoRepository : ISellerInfoRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public SellerInfoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<SellerInfo> GetByIdAsync(Guid id)
        {
            return await _appDbContext.SellerInfos.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
