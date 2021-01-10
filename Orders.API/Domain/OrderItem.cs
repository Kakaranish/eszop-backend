using System;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Domain;
using Common.Validators;

namespace Orders.API.Domain
{
    public class OrderItem : EntityBase, IAggregateRoot
    {
        public Guid OfferId { get; private set; }
        public string OfferName { get; private set; }
        public int Quantity { get; private set; }
        public decimal PricePerItem { get; private set; }
        [NotMapped] public decimal TotalPrice => Quantity * PricePerItem;

        public OrderItem(Guid offerId, string offerName, int quantity, decimal pricePerItem)
        {
            SetOfferId(offerId);
            SetOfferName(offerName);
            SetQuantity(quantity);
            SetPricePerItem(pricePerItem);
        }
        
        public void SetOfferId(Guid offerId)
        {
            ValidateOfferId(offerId);
            OfferId = offerId;
        }

        public void SetOfferName(string offerName)
        {
            ValidateOfferName(offerName);
            OfferName = offerName;
        }

        public void SetQuantity(int quantity)
        {
            ValidateQuantity(quantity);
            Quantity = quantity;
        }

        public void SetPricePerItem(decimal pricePerItem)
        {
            ValidatePricePerItem(pricePerItem);
            PricePerItem = pricePerItem;
        }

        #region Validation

        public void ValidateOfferId(Guid offerId)
        {
            var validator = new IdValidator();
            var result = validator.Validate(offerId);
            if (!result.IsValid) throw new OrdersDomainException($"'{nameof(offerId)}' is invalid id");
        }

        public void ValidateOfferName(string offerName)
        {
            var validator = new OfferNameValidator();
            var result = validator.Validate(offerName);
            if (!result.IsValid) throw new OrdersDomainException($"'{nameof(offerName)}' is invalid name");
        }

        public void ValidateQuantity(int quantity)
        {
            if (quantity <= 0) throw new OrdersDomainException($"'{nameof(quantity)}' must be > 0");
        }
        
        public void ValidatePricePerItem(decimal pricePerItem)
        {
            var validator = new OfferPriceValidator();
            var result = validator.Validate(pricePerItem);
            if (!result.IsValid) throw new OrdersDomainException($"'{nameof(pricePerItem)}' is invalid price");
        }

        #endregion
    }
}
