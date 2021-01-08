using Common.DataAccess;
using Identity.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.DataAccess.Repositories
{
    public interface IDeliveryAddressRepository : IDomainRepository<DeliveryAddress>
    {
        Task<DeliveryAddress> GetById(Guid id);
        Task<IList<DeliveryAddress>> GetByUserId(Guid userId);
        void Add(DeliveryAddress deliveryAddress);
        void Update(DeliveryAddress deliveryAddress);
        void Remove(DeliveryAddress deliveryAddress);
    }
}
