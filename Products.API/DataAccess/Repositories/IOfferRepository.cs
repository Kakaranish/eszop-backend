using System.Collections.Generic;
using System.Threading.Tasks;
using Products.API.Domain;

namespace Products.API.DataAccess.Repositories
{
    public interface IOfferRepository
    {
        Task AddAsync(Offer product);
        IEnumerable<Offer> GetAll();
    }
}
