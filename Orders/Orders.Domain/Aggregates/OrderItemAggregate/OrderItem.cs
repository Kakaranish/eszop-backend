using Common.Domain;
using Orders.Domain.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orders.Domain.Aggregates.OrderItemAggregate
{
    public class OrderItem : EntityBase, IAggregateRoot
    {
        public int Quantity { get; private set; }
        public OfferDetails OfferDetails { get; private set; }

        [NotMapped] public decimal TotalPrice => Quantity * OfferDetails.PricePerItem;

        private OrderItem()
        {
        }

        public OrderItem(OfferDetails offerDetails, int quantity)
        {
            SetOfferDetails(offerDetails);
            SetQuantity(quantity);
        }

        public void SetOfferDetails(OfferDetails offerDetails)
        {
            ValidateOfferDetails(offerDetails);
            OfferDetails = offerDetails;
        }

        public void SetQuantity(int quantity)
        {
            ValidateQuantity(quantity);
            Quantity = quantity;
        }

        #region Validation

        private void ValidateOfferDetails(OfferDetails offerDetails)
        {
            if (offerDetails == null)
                throw new OrdersDomainException($"{nameof(offerDetails)} cannot be null");
        }

        public void ValidateQuantity(int quantity)
        {
            if (quantity <= 0) throw new OrdersDomainException($"'{nameof(quantity)}' must be > 0");
        }

        #endregion
    }
}
