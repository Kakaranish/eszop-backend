using System;

namespace Common.Dto
{
    public class CartItemDto
    {
        public Guid OfferId { get; init; }
        public Guid CartId { get; init; }
        public Guid SellerId { get; init; }
        public string OfferName { get; init; }
        public int Quantity { get; init; }
        public decimal PricePerItem { get; init; }
    }
}
