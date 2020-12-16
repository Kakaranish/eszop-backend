using Common.Types;
using Identity.API.Domain;
using System;
using System.Threading.Tasks;

namespace Identity.API.DataAccess.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByIdAsync(Guid id);
        Task<User> FindByEmailAsync(string email);
        Task AddUserAsync(User user);
    }
}
