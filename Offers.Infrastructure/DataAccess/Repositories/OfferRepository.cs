using Common.Domain.Repositories;
using Common.Extensions;
using Common.Types;
using Microsoft.EntityFrameworkCore;
using Offers.Domain.Aggregates.OfferAggregate;
using Offers.Domain.Repositories;
using Offers.Domain.Repositories.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Offers.Infrastructure.Extensions;

namespace Offers.Infrastructure.DataAccess.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private static readonly Expression<Func<Offer, bool>> OfferActiveExpression = offer
            => offer.PublishedAt != null && offer.UserEndedAt == null && offer.EndsAt > DateTime.UtcNow && offer.RemovedAt == null;

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
                .Where(OfferActiveExpression)
                .ApplyFilter(filter)
                .OrderBy(x => x.EndsAt)
                .Include(x => x.Category);

            var pageDetails = new PageCriteria(filter.PageIndex, filter.PageSize);

            return await offers.PaginateAsync(pageDetails);
        }

        public async Task<IList<Offer>> GetMultipleWithIds(IEnumerable<Guid> offerIds)
        {
            var offers = _appDbContext.Offers.AsQueryable()
                .Where(x => offerIds.Contains(x.Id) && x.RemovedAt == null);

            return await offers.ToListAsync();
        }

        public async Task<Pagination<Offer>> GetAllByUserIdAsync(Guid userId, OfferFilter filter)
        {
            var offers = _appDbContext.Offers.AsQueryable()
                .Where(x => x.OwnerId == userId && x.RemovedAt == null)
                .OrderByDescending(x => x.PublishedAt == null)
                .ThenByDescending(x => x.CreatedAt)
                .ApplyFilter(filter);

            var pageDetails = new PageCriteria(filter.PageIndex, filter.PageSize);

            return await offers.PaginateAsync(pageDetails);
        }

        public async Task<Pagination<Offer>> GetAllActiveByUserIdAsync(Guid userId, OfferFilter filter)
        {
            var offers = _appDbContext.Offers.AsQueryable()
                .Where(x => x.OwnerId == userId)
                .Where(OfferActiveExpression)
                .OrderBy(x => x.EndsAt)
                .ApplyFilter(filter);

            var pageDetails = new PageCriteria(filter.PageIndex, filter.PageSize);

            return await offers.PaginateAsync(pageDetails);
        }

        public async Task<Offer> GetPublishedById(Guid offerId)
        {
            return await _appDbContext.Offers.Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == offerId && x.PublishedAt != null && x.RemovedAt == null);
        }

        public async Task<Offer> GetByIdAsync(Guid offerId)
        {
            return await _appDbContext.Offers.Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == offerId && x.RemovedAt == null);
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
