using Common.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Offers.Domain.Aggregates.CategoryAggregate;
using Offers.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Offers.Infrastructure.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public IUnitOfWork UnitOfWork => _appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<IList<Category>> GetAllAsync()
        {
            return await _appDbContext.Categories.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task AddAsync(Category category)
        {
            await _appDbContext.Categories.AddAsync(category);
        }

        public void Update(Category category)
        {
            _appDbContext.Update(category);
        }
    }
}
