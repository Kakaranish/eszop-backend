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
        public decimal PricePerItem { get; private set; }
        public int Quantity { get; private set; }
        public int AvailableStock { get; private set; }
        [NotMapped] public decimal TotalPrice => PricePerItem * Quantity;

        public CartItem(Guid cartId, Guid offerId, Guid sellerId, string offerName, decimal pricePerItem, int quantity, int availableStock)
        {
            // TODO: Validation
            
            CartId = cartId;
            OfferId = offerId;
            SellerId = sellerId;
            OfferName = offerName;
            PricePerItem = pricePerItem;
            Quantity = quantity;
            AvailableStock = availableStock;
        }

        public void SetPricePerItem(decimal pricePerItem)
        {
            if (pricePerItem == PricePerItem) return;
            ValidatePricePerItem(pricePerItem);

            PricePerItem = pricePerItem;
        }

        public void SetQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new CartsDomainException($"{nameof(quantity)} must be > 0");
            }
            if(quantity > AvailableStock)
            {
                throw new CartsDomainException($"Max {nameof(quantity)} of offer {OfferId} is {AvailableStock}");
            }

            Quantity = quantity;
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
