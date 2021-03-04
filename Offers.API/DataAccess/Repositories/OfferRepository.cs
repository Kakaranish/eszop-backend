using Common.DataAccess;
using Common.Extensions;
using Common.Types;
using Microsoft.EntityFrameworkCore;
using Offers.API.Application.Types;
using Offers.API.Domain;
using Offers.API.Extensions;
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

        public async Task<Pagination<Offer>> GetAllActiveAsync(OfferFilter filter)
        {
            var offers = _appDbContext.Offers
                .AsQueryable()
                .Where(x => x.PublishedAt != null)
                .ApplyFilter(filter)
                .OrderByDescending(x => x.CreatedAt)
                .Include(x => x.Category);

            var pageDetails = new PageCriteria(filter.PageIndex, filter.PageSize);

            return await offers.PaginateAsync(pageDetails);
        }

        public async Task<IList<Offer>> GetMultipleWithIds(IEnumerable<Guid> offerIds)
        {
            var offers = _appDbContext.Offers.AsQueryable()
                .Where(x => offerIds.Contains(x.Id));

            return await offers.ToListAsync();
        }

        public async Task<Pagination<Offer>> GetByUserIdAsync(Guid userId, OfferFilter filter)
        {
            var offers = _appDbContext.Offers.AsQueryable()
                .Where(x => x.OwnerId == userId)
                .OrderByDescending(x => x.PublishedAt == null)
                .ThenByDescending(x => x.CreatedAt)
                .ApplyFilter(filter);

            var pageDetails = new PageCriteria(filter.PageIndex, filter.PageSize);

            return await offers.PaginateAsync(pageDetails);
        }

        public async Task<Offer> GetByIdAsync(Guid offerId)
        {
            return await _appDbContext.Offers.Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == offerId);
        }

        public async Task AddAsync(Offer offer)
        {
            await _appDbContext.Offers.AddAsync(offer);
        }

        public void Update(Offer offer)
        {
            _appDbContext.Update(offer);
        }

        public void Remove(Offer offer)
        {
            _appDbContext.Remove(offer);
        }
    }
}
