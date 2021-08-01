using Common.Domain.Repositories;
using Offers.Domain.Aggregates.PredefinedDeliveryMethodAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.Domain.Repositories
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
