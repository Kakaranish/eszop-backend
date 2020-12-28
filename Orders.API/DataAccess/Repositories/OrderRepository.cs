using System;
using System.Threading.Tasks;
using Common.Types;
using Orders.API.Domain;

namespace Orders.API.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        
        public IUnitOfWork UnitOfWork => _appDbContext;
        
        public OrderRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task AddAsync(Order offer)
        {
            await _appDbContext.Orders.AddAsync(offer);
        }
    }
}
