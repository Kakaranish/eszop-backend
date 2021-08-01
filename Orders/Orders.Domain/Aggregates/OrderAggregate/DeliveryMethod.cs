using Common.Domain;
using Orders.Domain.Exceptions;

namespace Orders.Domain.Aggregates.OrderAggregate
{
    public class DeliveryMethod : EntityBase
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

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new OrdersDomainException($"{nameof(Name)} cannot be null or whitespace");
        }

        private void ValidatePrice(decimal price)
        {
            if (price < 0)
                throw new OrdersDomainException($"{nameof(Price)} cannot be less than 0");
        }

        #endregion
    }
}
