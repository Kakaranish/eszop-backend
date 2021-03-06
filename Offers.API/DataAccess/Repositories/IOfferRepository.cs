using Common.DataAccess;
using Common.Types;
using Offers.API.Application.Types;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.API.DataAccess.Repositories
{
    public interface IOfferRepository : IDomainRepository<Offer>
    {
        Task<Pagination<Offer>> GetAllActiveAsync(OfferFilter filter);
        Task<IList<Offer>> GetMultipleWithIds(IEnumerable<Guid> offerIds);
        Task<Pagination<Offer>> GetAllByUserIdAsync(Guid userId, OfferFilter filter);
        Task<Pagination<Offer>> GetAllActiveByUserIdAsync(Guid userId, OfferFilter filter);
        Task<Offer> GetByIdAsync(Guid offerId);
        Task AddAsync(Offer product);
        void Update(Offer offer);
        void Remove(Offer offer);
    }
}