using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OrdersService.CreateOrder
{
    [DataContract]
    public class CreateOrderRequest
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public Guid UserId { get; set; }

        [DataMember(Order = 3)]
        public Guid SellerId { get; set; }

        [DataMember(Order = 4)]
        public IEnumerable<CartItemDto> CartItems { get; set; }
    }
}
