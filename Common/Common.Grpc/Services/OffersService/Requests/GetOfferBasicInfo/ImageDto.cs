using System;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OffersService.Requests.GetOfferBasicInfo
{
    [DataContract]
    public class ImageDto
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public string Uri { get; set; }

        [DataMember(Order = 3)]
        public bool IsMain { get; set; }
    }
}
