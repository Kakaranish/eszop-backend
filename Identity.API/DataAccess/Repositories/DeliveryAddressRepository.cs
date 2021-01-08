using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DataAccess;
using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.DataAccess.Repositories
{
    public class DeliveryAddressRepository : IDeliveryAddressRepository
    {
        private readonly AppDbContext _appDbContext;
        
        public IUnitOfWork UnitOfWork => _appDbContext;

        public DeliveryAddressRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<DeliveryAddress> GetById(Guid id)
        {
            return await _appDbContext.DeliveryAddresses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<DeliveryAddress>> GetByUserId(Guid userId)
        {
            return await _appDbContext.DeliveryAddresses.Where(x => x.UserId == userId).ToListAsync();
        }

        public void Add(DeliveryAddress deliveryAddress)
        {
            _appDbContext.DeliveryAddresses.Add(deliveryAddress);
        }

        public void Update(DeliveryAddress deliveryAddress)
        {
            _appDbContext.Update(deliveryAddress);
        }

        public void Remove(DeliveryAddress deliveryAddress)
        {
            _appDbContext.RemoveRange(deliveryAddress);
        }
    }
}
