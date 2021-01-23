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
        Task<Pagination<Offer>> GetFiltered(OfferFilter filter);
        Task<IList<Offer>> GetAllPublishedAsync();
        Task<Pagination<Offer>> GetByUserIdAsync(Guid userId, OfferFilter filter);
        Task<Offer> GetByIdAsync(Guid offerId);
        Task AddAsync(Offer product);
        void Update(Offer offer);
    }
}