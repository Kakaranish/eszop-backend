using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OffersService.Requests.GetOfferBasicInfo
{
    [DataContract]
    public class OfferDto
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public Guid OwnerId { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        public decimal Price { get; set; }

        [DataMember(Order = 5)]
        public int AvailableStock { get; set; }

        [DataMember(Order = 6)]
        public IEnumerable<ImageDto> Images { get; set; }
    }
}
