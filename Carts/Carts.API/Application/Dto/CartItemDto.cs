using System;

namespace Carts.API.Application.Dto
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid CartId { get; init; }
        public Guid CartOwnerId { get; init; }
        public Guid OfferId { get; init; }
        public Guid SellerId { get; init; }
        public string OfferName { get; init; }
        public int Quantity { get; init; }
        public int AvailableStock { get; init; }
        public decimal PricePerItem { get; init; }
        public string ImageUri { get; init; }
    }
}