using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Common.DataAccess;

namespace Identity.API.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public void AddUser(User user)
        {
            _appDbContext.Add(user);
        }

        public void Update(User user)
        {
            _appDbContext.Update(user);
        }
    }
}
