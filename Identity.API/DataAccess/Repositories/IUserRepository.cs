using Identity.API.Domain;
using System;
using System.Threading.Tasks;
using Common.DataAccess;

namespace Identity.API.DataAccess.Repositories
{
    public interface IUserRepository : IDomainRepository<User>
    {
        Task<User> FindByIdAsync(Guid id);
        Task<User> FindByEmailAsync(string email);
        void AddUser(User user);
        void Update(User user);
    }
}
