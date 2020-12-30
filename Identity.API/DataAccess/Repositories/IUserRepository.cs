using Identity.API.Domain;
using System;
using System.Threading.Tasks;
using Common.DataAccess;

namespace Identity.API.DataAccess.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByIdAsync(Guid id);
        Task<User> FindByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateAsync(User user);
    }
}
