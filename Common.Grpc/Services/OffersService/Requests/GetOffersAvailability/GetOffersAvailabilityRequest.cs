using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OffersService.Requests.GetOffersAvailability
{
    [DataContract]
    public class GetOffersAvailabilityRequest
    {
        [DataMember(Order = 1)]
        public IList<Guid> OfferIds { get; set; }
    }
}
