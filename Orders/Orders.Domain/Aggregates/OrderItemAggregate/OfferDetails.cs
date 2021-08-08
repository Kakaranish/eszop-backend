using Common.Domain;
using Common.Domain.Validators;
using Orders.Domain.Exceptions;
using System;

namespace Orders.Domain.Aggregates.OrderItemAggregate
{
    public class OfferDetails : EntityBase
    {
        public string Name { get; private set; }
        public decimal PricePerItem { get; private set; }
        public string ImageUri { get; private set; }

        private OfferDetails()
        {
        }

        public OfferDetails(Guid id, string name, decimal pricePerItem, string imageUri)
        {
            SetId(id);
            SetName(name);
            SetPricePerItem(pricePerItem);
            SetImageUri(imageUri);
        }

        private void SetId(Guid id)
        {
            ValidateOfferId(id);
            Id = id;
        }

        public void SetName(string name)
        {
            ValidateOfferName(name);
            Name = name;
        }

        public void SetPricePerItem(decimal pricePerItem)
        {
            ValidatePricePerItem(pricePerItem);
            PricePerItem = pricePerItem;
        }

        public void SetImageUri(string imageUri)
        {
            ValidateImageUri(imageUri);
            ImageUri = imageUri;
        }

        #region Validation

        private void ValidateOfferId(Guid id)
        {
            var validator = new IdValidator();
            var result = validator.Validate(id);
            if (!result.IsValid) throw new OrdersDomainException($"'{nameof(id)}' is invalid id");
        }

        private void ValidateOfferName(string name)
        {
            var validator = new OfferNameValidator();
            var result = validator.Validate(name);
            if (!result.IsValid) throw new OrdersDomainException($"'{nameof(name)}' is invalid name");
        }

        private void ValidatePricePerItem(decimal pricePerItem)
        {
            var validator = new OfferPriceValidator();
            var result = validator.Validate(pricePerItem);
            if (!result.IsValid) throw new OrdersDomainException($"'{nameof(pricePerItem)}' is invalid price");
        }

        private static void ValidateImageUri(string imageUri)
        {
            if (string.IsNullOrWhiteSpace(imageUri))
                throw new OrdersDomainException($"{nameof(imageUri)} cannot be null or empty");
            if (!Uri.IsWellFormedUriString(imageUri, UriKind.RelativeOrAbsolute))
                throw new OrdersDomainException($"{nameof(imageUri)} is not well formed uri");
        }

        #endregion
    }
}
