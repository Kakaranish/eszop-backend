using Common.Domain;

namespace Offers.API.Domain
{
    public class PredefinedDeliveryMethod : EntityBase, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public PredefinedDeliveryMethod(string name, string description)
        {
            SetName(name);
            SetDescription(description);
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

        #region Validation

        public void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new OffersDomainException($"{nameof(Name)} cannot be null or whitespace");
        }

        #endregion
    }
}
