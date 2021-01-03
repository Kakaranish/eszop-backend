using Common.DataAccess;
using Common.Extensions;
using Common.Types;
using Microsoft.EntityFrameworkCore;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Offers.API.DataAccess.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public OfferRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<IList<Offer>> GetAllAsync()
        {
            return await _appDbContext.Offers.Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<Offer> GetByIdAsync(Guid offerId)
        {
            return await _appDbContext.Offers.Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == offerId);
        }

        public async Task<Pagination<Offer>> GetFiltered(OfferFilter filter, PageDetails pageDetails)
        {
            var offers = _appDbContext.Offers.AsQueryable();

            offers = offers.OrderByDescending(x => x.CreatedAt);
            if (filter.PriceFrom != null) offers = offers.Where(x => x.Price >= filter.PriceFrom);
            if (filter.PriceTo != null) offers = offers.Where(x => x.Price <= filter.PriceTo);
            if (filter.Category != null) offers = offers.Where(x => x.Category.Id == filter.Category);

            return await offers.PaginateAsync(pageDetails);
        }

        public async Task AddAsync(Offer offer)
        {
            await _appDbContext.Offers.AddAsync(offer);
        }

        public void Update(Offer offer)
        {
            _appDbContext.Update(offer);
        }
    }
}
