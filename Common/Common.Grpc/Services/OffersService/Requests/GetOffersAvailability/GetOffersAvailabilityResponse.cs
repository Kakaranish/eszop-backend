using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OffersService.Requests.GetOffersAvailability
{
    [DataContract]
    public class GetOffersAvailabilityResponse
    {
        [DataMember(Order = 1)]
        public IList<OfferAvailability> OfferAvailabilities { get; set; }
    }
}
