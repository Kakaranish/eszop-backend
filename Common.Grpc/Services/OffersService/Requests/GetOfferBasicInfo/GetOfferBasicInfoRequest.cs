using System;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OffersService.Requests.GetOfferBasicInfo
{
    [DataContract]
    public class GetOfferBasicInfoRequest
    {
        [DataMember(Order = 1)]
        public Guid OfferId { get; set; }
    }
}
