using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Identity.API.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> FindByIdAsync(Guid id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _appDbContext.Update(user);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
