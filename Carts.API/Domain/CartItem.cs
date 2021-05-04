using System;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Domain;
using Common.Types;
using Common.Validators;

namespace Carts.API.Domain
{
    public class CartItem : EntityBase, IAggregateRoot, ITimeStamped
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public Guid CartId { get; private set; }
        public Guid CartOwnerId { get; private set; }
        public Guid OfferId { get; private set; }
        public Guid SellerId { get; private set; }
        public string OfferName { get; private set; }
        public decimal PricePerItem { get; private set; }
        public int Quantity { get; private set; }
        public int AvailableStock { get; private set; }
        public string ImageUri { get; private set; }
        [NotMapped] public decimal TotalPrice => PricePerItem * Quantity;

        public CartItem(Guid cartId, Guid cartOwnerId, Guid offerId, Guid sellerId, string offerName,
            decimal pricePerItem,
            int quantity, int availableStock, string imageUri)
        {
            SetCartId(cartId);
            SetCartOwnerId(cartOwnerId);
            SetOfferId(offerId);
            SetSellerId(sellerId);
            SetOfferName(offerName);
            SetPricePerItem(pricePerItem);
            AvailableStock = availableStock;
            SetQuantity(quantity);
            SetImageUri(imageUri);
            
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        private void SetCartId(Guid cartId)
        {
            ValidateCartId(cartId);
            CartId = cartId;
        }

        private void SetCartOwnerId(Guid cartOwnerId)
        {
            ValidateCartOwnerId(cartOwnerId);
            CartOwnerId = cartOwnerId;
        }

        private void SetOfferId(Guid offerId)
        {
            ValidateOfferId(offerId);
            OfferId = offerId;
        }

        private void SetSellerId(Guid sellerId)
        {
            ValidateSellerId(sellerId);
            SellerId = sellerId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetOfferName(string offerName)
        {
            ValidateOfferName(offerName);
            OfferName = offerName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPricePerItem(decimal pricePerItem)
        {
            ValidatePricePerItem(pricePerItem);
            PricePerItem = pricePerItem;
            UpdatedAt = DateTime.UtcNow;
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
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAvailableStock(int availableStock)
        {
            ValidateAvailableStock(availableStock);
            
            if (availableStock < Quantity)
            {
                // TODO: Generate domain event
            }

            AvailableStock = availableStock;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetImageUri(string imageUri)
        {
            ValidateImageUri(imageUri);
            ImageUri = imageUri;
            UpdatedAt = DateTime.UtcNow;
        }

        #region Validation

        private static void ValidateCartId(Guid cartId)
        {
            ValidateIdAndThrow(cartId, nameof(cartId));
        }

        private static void ValidateCartOwnerId(Guid cartOwnerId)
        {
            ValidateIdAndThrow(cartOwnerId, nameof(CartOwnerId));
        }

        private static void ValidateOfferId(Guid offerId)
        {
            ValidateIdAndThrow(offerId, nameof(OfferId));
        }
        
        private static void ValidateSellerId(Guid sellerId)
        {
            ValidateIdAndThrow(sellerId, nameof(SellerId));
        }

        private static void ValidateOfferName(string offerName)
        {
            var validator = new OfferNameValidator();
            var result = validator.Validate(offerName);
            if (!result.IsValid) throw new CartsDomainException($"{nameof(OfferName)} is invalid offer name");
        }
        
        private static void ValidatePricePerItem(decimal pricePerItem)
        {
            if (pricePerItem <= 0) throw new CartsDomainException($"{nameof(PricePerItem)} must be >= 0");
        }

        private static void ValidateAvailableStock(int availableStock)
        {
            if (availableStock < 0)
                throw new CartsDomainException($"{nameof(AvailableStock)} cannot be < 0");
        }

        private static void ValidateImageUri(string imageUri)
        {
            if (string.IsNullOrWhiteSpace(imageUri))
                throw new CartsDomainException($"{nameof(ImageUri)} cannot be null or empty");
            if (!Uri.IsWellFormedUriString(imageUri, UriKind.RelativeOrAbsolute))
                throw new CartsDomainException($"{nameof(ImageUri)} is not well formed uri");
        }

        private static void ValidateIdAndThrow(Guid id, string paramName)
        {
            var idValidator = new IdValidator();
            var result = idValidator.Validate(id);
            if (!result.IsValid) throw new CartsDomainException($"'{paramName}' is invalid id");
        }

        #endregion
    }
}
