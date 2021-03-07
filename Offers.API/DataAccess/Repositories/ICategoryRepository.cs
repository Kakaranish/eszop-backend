using Common.DataAccess;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.API.DataAccess.Repositories
{
    public interface ICategoryRepository : IDomainRepository<Category>
    {
        Task<IList<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(Guid id);
        Task<Category> GetByNameAsync(string name);
        Task AddAsync(Category category);
        void Update(Category category);
    }
}
