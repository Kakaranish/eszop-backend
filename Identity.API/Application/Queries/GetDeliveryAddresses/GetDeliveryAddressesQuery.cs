using Identity.API.Application.Dto;
using MediatR;
using System.Collections.Generic;

namespace Identity.API.Application.Queries.GetDeliveryAddresses
{
    public class GetDeliveryAddressesQuery : IRequest<IList<DeliveryAddressDto>>
    {
    }
}
