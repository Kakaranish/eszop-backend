using Common.DataAccess;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Offers.API.DataAccess.Repositories
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
            return await _appDbContext.Categories.ToListAsync();
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
    }
}
