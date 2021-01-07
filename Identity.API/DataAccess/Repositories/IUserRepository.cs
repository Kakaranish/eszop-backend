using Identity.API.Domain;
using System;
using System.Threading.Tasks;
using Common.DataAccess;

namespace Identity.API.DataAccess.Repositories
{
    public interface IUserRepository : IDomainRepository<User>
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        void AddUser(User user);
        void Update(User user);
    }
}
