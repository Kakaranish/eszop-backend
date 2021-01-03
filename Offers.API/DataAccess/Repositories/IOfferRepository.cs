using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DataAccess;
using Common.Types;
using Offers.API.Domain;

namespace Offers.API.DataAccess.Repositories
{
    public interface IOfferRepository : IDomainRepository<Offer>
    {
        Task<IList<Offer>> GetAllAsync();
        Task<Offer> GetByIdAsync(Guid offerId);
        Task<Pagination<Offer>> GetFiltered(OfferFilter filter, PageDetails pageDetails);
        Task AddAsync(Offer product);
        void Update(Offer offer);
    }
}