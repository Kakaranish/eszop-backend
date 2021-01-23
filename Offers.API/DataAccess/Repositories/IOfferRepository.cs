using Common.DataAccess;
using Common.Types;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.API.DataAccess.Repositories
{
    public interface IOfferRepository : IDomainRepository<Offer>
    {
        Task<IList<Offer>> GetAllAsync();
        Task<IList<Offer>> GetAllPublishedAsync();
        Task<IList<Offer>> GetAllByUserIdAsync(Guid userId);
        Task<Offer> GetByIdAsync(Guid offerId);
        Task<Pagination<Offer>> GetFiltered(OfferFilter filter, PageDetails pageDetails);
        Task AddAsync(Offer product);
        void Update(Offer offer);
    }
}