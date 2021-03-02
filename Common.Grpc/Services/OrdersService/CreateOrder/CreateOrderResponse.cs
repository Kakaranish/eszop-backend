using System;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OrdersService.CreateOrder
{
    [DataContract]
    public class CreateOrderResponse
    {
        [DataMember(Order = 1)]
        public Guid OrderId { get; set; }
    }
}
