using System;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OffersService.Requests.GetOffersAvailability
{
    [DataContract]
    public class OfferAvailability
    {
        [DataMember(Order = 1)]
        public Guid OfferId { get; set; }

        [DataMember(Order = 2)]
        public Availability Availability { get; set; }
    }
}
