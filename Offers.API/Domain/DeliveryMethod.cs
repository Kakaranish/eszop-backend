﻿namespace Offers.API.Domain
{
    public class DeliveryMethod
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public DeliveryMethod(string name, decimal price)
        {
            SetName(name);
            SetPrice(price);
        }

        public void SetName(string name)
        {
            ValidateName(name);
            Name = name;
        }

        public void SetPrice(decimal price)
        {
            ValidatePrice(price);
            Price = price;
        }

        #region Validation

        public void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new OffersDomainException($"{nameof(Name)} cannot be null or whitespace");
        }

        public void ValidatePrice(decimal price)
        {
            if (price < 0) throw new OffersDomainException($"{nameof(Price)} must be >= 0");
        }

        #endregion
    }
}