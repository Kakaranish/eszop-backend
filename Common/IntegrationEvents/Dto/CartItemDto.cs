using System;

namespace Common.IntegrationEvents.Dto
{
    public class CartItemDto
    {
        public Guid OfferId { get; init; }
        public string OfferName { get; init; }
        public int Quantity { get; init; }
        public decimal PricePerItem { get; init; }
    }
}
