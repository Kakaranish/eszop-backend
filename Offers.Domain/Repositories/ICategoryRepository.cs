using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Domain.Repositories;
using Offers.Domain.Aggregates.CategoryAggregate;

namespace Offers.Domain.Repositories
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
