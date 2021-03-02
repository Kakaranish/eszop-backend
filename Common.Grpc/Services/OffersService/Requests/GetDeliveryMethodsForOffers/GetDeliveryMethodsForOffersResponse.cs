using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OffersService.Requests.GetDeliveryMethodsForOffers
{
    [DataContract]
    public class GetDeliveryMethodsForOffersResponse
    {
        [DataMember(Order = 1)]
        public IEnumerable<DeliveryMethodDto> DeliveryMethods { get; set; }
    }
}
