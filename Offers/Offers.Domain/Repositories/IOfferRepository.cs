using Common.Domain.Repositories;
using Common.Domain.Types;
using Offers.Domain.Aggregates.OfferAggregate;
using Offers.Domain.Repositories.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.Domain.Repositories
{
    public interface IOfferRepository : IDomainRepository<Offer>
    {
        Task<Pagination<Offer>> GetAllActiveAsync(OfferFilter filter);
        Task<IList<Offer>> GetMultipleWithIds(IEnumerable<Guid> offerIds);
        Task<Pagination<Offer>> GetAllByUserIdAsync(Guid userId, OfferFilter filter);
        Task<Pagination<Offer>> GetAllActiveByUserIdAsync(Guid userId, OfferFilter filter);
        Task<Offer> GetPublishedById(Guid offerId);
        Task<Offer> GetByIdAsync(Guid offerId);
        Task AddAsync(Offer product);
        void Update(Offer offer);
        void Remove(Offer offer);
    }
}