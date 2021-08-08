using Common.Domain;
using Common.Domain.Validators;
using Orders.Domain.Aggregates.DeliveryAddressAggregate;
using Orders.Domain.Exceptions;
using System;

namespace Orders.Domain.Aggregates.OrderAggregate
{
    public class Buyer : EntityBase
    {
        public virtual DeliveryAddress DeliveryAddress { get; private set; }

        protected Buyer()
        {
        }

        public Buyer(Guid id)
        {
            SetBuyerId(id);
        }

        private void SetBuyerId(Guid buyerId)
        {
            ValidateBuyerId(buyerId);
            Id = buyerId;
        }

        public void SetDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            ValidateDeliveryAddress(deliveryAddress);

            DeliveryAddress = deliveryAddress;
        }

        #region Validation

        private static void ValidateBuyerId(Guid buyerId)
        {
            var validator = new IdValidator();
            var result = validator.Validate(buyerId);
            if (!result.IsValid) throw new OrdersDomainException(nameof(buyerId));
        }

        private static void ValidateDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null)
                throw new OrdersDomainException($"'{nameof(deliveryAddress)}' cannot be null");
        }

        #endregion
    }
}
