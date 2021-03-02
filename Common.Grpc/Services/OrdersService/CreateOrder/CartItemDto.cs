using System;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.OrdersService.CreateOrder
{
    [DataContract]
    public class CartItemDto
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public Guid OfferId { get; set; }

        [DataMember(Order = 3)]
        public Guid CartId { get; set; }

        [DataMember(Order = 4)]
        public Guid SellerId { get; set; }

        [DataMember(Order = 5)]
        public string OfferName { get; set; }

        [DataMember(Order = 6)]
        public int Quantity { get; set; }

        [DataMember(Order = 7)]
        public int AvailableStock { get; set; }

        [DataMember(Order = 8)]
        public decimal PricePerItem { get; set; }

        [DataMember(Order = 9)]
        public string ImageUri { get; set; }
    }
}
