using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Products.API.Domain;

namespace Products.API.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task AddAsync(Product product)
        {
            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Product> GetAll()
        {
            return _appDbContext.Products;
        }
    }
}
