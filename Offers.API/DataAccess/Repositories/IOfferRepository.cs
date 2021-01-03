using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DataAccess;
using Offers.API.Domain;

namespace Offers.API.DataAccess.Repositories
{
    public interface IOfferRepository : IDomainRepository<Offer>
    {
        Task<IList<Offer>> GetAllAsync();
        Task<Offer> GetByIdAsync(Guid offerId);
        Task AddAsync(Offer product);
        void Update(Offer offer);
    }
}