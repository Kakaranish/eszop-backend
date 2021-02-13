using Common.Domain;

namespace Offers.API.Domain
{
    public class PredefinedDeliveryMethod : EntityBase, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal? Price { get; set; }

        protected PredefinedDeliveryMethod()
        {
        }

        public PredefinedDeliveryMethod(string name)
        {
            SetName(name);
        }

        public void SetName(string name)
        {
            ValidateName(name);
            Name = name;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetPrice(decimal? price)
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

        public void ValidatePrice(decimal? price)
        {
            if (price != null && price < 0)
                throw new OffersDomainException($"{nameof(Price)} cannot be less than 0");
        }

        #endregion
    }
}
