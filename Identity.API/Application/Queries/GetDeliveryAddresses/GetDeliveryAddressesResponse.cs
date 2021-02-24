using Identity.API.Application.Dto;
using System;
using System.Collections.Generic;

namespace Identity.API.Application.Queries.GetDeliveryAddresses
{
    public class GetDeliveryAddressesResponse
    {
        public IList<DeliveryAddressDto> DeliveryAddresses { get; init; }
        public Guid? PrimaryDeliveryAddressId { get; init; }
    }
}
