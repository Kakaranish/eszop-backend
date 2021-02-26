using Common.Dto;
using Orders.API.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orders.API.Application.Services
{
    public interface IDeliveryMethodsProvider
    {
        Task<IList<DeliveryMethodDto>> Get(Order order);
    }
}
