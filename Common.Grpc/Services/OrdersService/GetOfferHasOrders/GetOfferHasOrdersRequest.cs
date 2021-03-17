using System;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OrdersService.GetOfferHasOrders
{
    [DataContract]
    public class GetOfferHasOrdersRequest
    {
        [DataMember(Order = 1)]
        public Guid OfferId { get; set; }
    }
}
