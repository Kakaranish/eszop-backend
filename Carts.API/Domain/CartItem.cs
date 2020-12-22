using System;
using Common.Types;
using Common.Types.Domain;

namespace Carts.API.Domain
{
    public class CartItem : EntityBase, IAggregateRoot
    {
        public Guid CartId { get; set; }
        public Guid OfferId { get; set; }
        public Guid SellerId { get; set; }
        public string SellerEmail { get; set; }
        public string OfferName { get; set; }
        public string OfferPhotoUrl { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
