using Common.Domain.Repositories;
using Identity.Domain.Aggregates.SellerInfoAggregate;
using System;
using System.Threading.Tasks;

namespace Identity.Domain.Repositories
{
    public interface ISellerInfoRepository : IDomainRepository<SellerInfo>
    {
        Task<SellerInfo> GetByIdAsync(Guid id);
        Task<SellerInfo> GetByUserIdAsync(Guid userId);
        void Add(SellerInfo sellerInfo);
        void Update(SellerInfo sellerInfo);
    }
}
