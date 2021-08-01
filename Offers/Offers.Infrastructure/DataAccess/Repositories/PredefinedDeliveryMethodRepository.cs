using Common.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Offers.Domain.Aggregates.PredefinedDeliveryMethodAggregate;
using Offers.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.Infrastructure.DataAccess.Repositories
{
    public class PredefinedDeliveryMethodRepository : IPredefinedDeliveryMethodRepository
    {
        private readonly AppDbContext _appDbContext;
        public IUnitOfWork UnitOfWork => _appDbContext;

        public PredefinedDeliveryMethodRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<IList<PredefinedDeliveryMethod>> GetAll()
        {
            return await _appDbContext.PredefinedDeliveryMethods.ToListAsync();
        }

        public async Task<PredefinedDeliveryMethod> GetById(Guid id)
        {
            return await _appDbContext.PredefinedDeliveryMethods.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Add(PredefinedDeliveryMethod predefinedDeliveryMethod)
        {
            _appDbContext.PredefinedDeliveryMethods.Add(predefinedDeliveryMethod);
        }

        public void Update(PredefinedDeliveryMethod predefinedDeliveryMethod)
        {
            _appDbContext.Update(predefinedDeliveryMethod);
        }

        public void Remove(PredefinedDeliveryMethod predefinedDeliveryMethod)
        {
            _appDbContext.PredefinedDeliveryMethods.Remove(predefinedDeliveryMethod);
        }
    }
}
