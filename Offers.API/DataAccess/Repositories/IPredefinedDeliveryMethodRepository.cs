using Common.DataAccess;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.API.DataAccess.Repositories
{
    public interface IPredefinedDeliveryMethodRepository : IDomainRepository<PredefinedDeliveryMethod>
    {
        Task<IList<PredefinedDeliveryMethod>> GetAll();
        Task<PredefinedDeliveryMethod> GetById(Guid id);
        void Add(PredefinedDeliveryMethod predefinedDeliveryMethod);
        void Update(PredefinedDeliveryMethod predefinedDeliveryMethod);
        void Remove(PredefinedDeliveryMethod predefinedDeliveryMethod);
    }
}
