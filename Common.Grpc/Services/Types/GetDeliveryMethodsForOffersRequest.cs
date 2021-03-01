using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.Types
{
    [DataContract]
    public class GetDeliveryMethodsForOffersRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<Guid> OfferIds { get; set; }
    }
}
