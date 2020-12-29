using Common.Types.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carts.API.Domain
{
    public class CartItem : EntityBase, IAggregateRoot
    {
        public Guid CartId { get; private set; }
        public Guid OfferId { get; private set; }
        public Guid SellerId { get; private set; }
        public string OfferName { get; private set; }
        public int Quantity { get; private set; }
        public decimal PricePerItem { get; private set; }

        [NotMapped] public decimal TotalPrice => PricePerItem * Quantity;

        public CartItem(Guid cartId, Guid offerId, Guid sellerId, string offerName, int quantity, decimal pricePerItem)
        {
            CartId = cartId;
            OfferId = offerId;
            SellerId = sellerId;
            OfferName = offerName;
            Quantity = quantity;
            PricePerItem = pricePerItem;
        }

        public void SetPricePerItem(decimal pricePerItem)
        {
            if (pricePerItem == PricePerItem) return;
            ValidatePricePerItem(pricePerItem);

            PricePerItem = pricePerItem;
        }

        public void SetOfferName(string offerName)
        {
            // TODO: Add validation
            OfferName = offerName;
        }

        public void ValidatePricePerItem(decimal pricePerItem)
        {
            if (pricePerItem <= 0) throw new CartsDomainException($"{nameof(pricePerItem)} must be >= 0");
        }
    }
}
