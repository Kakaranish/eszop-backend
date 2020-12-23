using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Types;
using Offers.API.Domain;

namespace Offers.API.DataAccess.Repositories
{
    public interface IOfferRepository : IDomainRepository<Offer>
    {
        Task AddAsync(Offer product);
        Task<Offer> GetByIdAsync(Guid offerId);
        Task<IList<Offer>> GetAllAsync();
    }
}