using System.Runtime.Serialization;

namespace Common.Grpc.Services.OffersService.Requests.GetOfferBasicInfo
{
    [DataContract]
    public class GetOfferBasicInfoResponse
    {
        [DataMember(Order = 1)]
        public OfferDto Offer { get; set; }
    }
}
