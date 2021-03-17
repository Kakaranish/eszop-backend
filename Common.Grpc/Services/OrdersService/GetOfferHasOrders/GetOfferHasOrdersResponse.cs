using System.Runtime.Serialization;

namespace Common.Grpc.Services.OrdersService.GetOfferHasOrders
{
    [DataContract]
    public class GetOfferHasOrdersResponse
    {
        [DataMember(Order = 1)]
        public bool OfferHasOrders { get; set; }
    }
}
