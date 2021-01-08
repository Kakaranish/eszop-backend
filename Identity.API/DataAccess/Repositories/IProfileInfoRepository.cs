using Common.DataAccess;
using Identity.API.Domain;
using System;
using System.Threading.Tasks;

namespace Identity.API.DataAccess.Repositories
{
    public interface IProfileInfoRepository : IDomainRepository<ProfileInfo>
    {
        Task<ProfileInfo> GetByUserIdAsync(Guid userId);
        void Add(ProfileInfo profileInfo);
        void Update(ProfileInfo profileInfo);
    }
}
