using System.Collections.Generic;
using System.Threading.Tasks;
using Products.API.Domain;

namespace Products.API.DataAccess.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        IEnumerable<Product> GetAll();
    }
}
