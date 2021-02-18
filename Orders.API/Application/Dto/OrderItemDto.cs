using System;

namespace Orders.API.Application.Dto
{
    public class OrderItemDto
    {
        public Guid Id { get; init; }
        public Guid OfferId { get; init; }
        public string OfferName { get; init; }
        public int Quantity { get; init; }
        public decimal PricePerItem { get; init; }
    }
}
