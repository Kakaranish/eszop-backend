using Common.Domain;
using Common.Domain.Types;
using Common.Domain.Validators;
using Common.Dto;
using Orders.API.Application.DomainEvents.OrderCancelled;
using Orders.API.Application.DomainEvents.OrderConfirmed;
using Orders.API.Application.DomainEvents.OrderStatusChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Orders.API.Domain
{
    public class Order : EntityBase, IAggregateRoot, ITimeStamped
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private List<OrderItem> _orderItems;
        public Guid BuyerId { get; private set; }
        public Guid SellerId { get; private set; }
        public OrderState OrderState { get; private set; }
        public virtual IReadOnlyCollection<OrderItem> OrderItems => _orderItems ?? new List<OrderItem>();
        public virtual DeliveryAddress DeliveryAddress { get; private set; }
        public virtual DeliveryMethod DeliveryMethod { get; private set; }

        [NotMapped] public bool IsCancelled => OrderState?.IsCancelled() ?? false;
        [NotMapped] public bool IsEditable => !IsCancelled && OrderState != OrderState.Shipped;
        [NotMapped] public decimal TotalPrice => OrderItems.Sum(orderItem => orderItem.TotalPrice);

        protected Order()
        {
        }

        public Order(Guid buyerId, Guid sellerId, IList<OrderItem> orderItems)
        {
            OrderState = OrderState.Started;

            SetBuyerId(buyerId);
            SetSellerId(sellerId);
            SetOrderItems(orderItems);

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        private void SetBuyerId(Guid buyerId)
        {
            ValidateBuyerId(buyerId);

            BuyerId = buyerId;
        }

        private void SetSellerId(Guid sellerId)
        {
            ValidateSellerId(sellerId);

            SellerId = sellerId;
        }

        private void SetOrderItems(IList<OrderItem> orderItems)
        {
            ValidateOrderItems(orderItems);

            _orderItems = orderItems.ToList();
        }

        public void SetCancelled(OrderState orderState)
        {
            ValidateCancellation(orderState);

            var previousState = OrderState;

            OrderState = orderState;
            UpdatedAt = DateTime.UtcNow;

            var domainEvent = new OrderCancelledDomainEvent
            {
                OrderId = Id,
                BuyerId = BuyerId,
                SellerId = SellerId,
                PreviousState = previousState,
                CurrentState = orderState,
                OrderItems = OrderItems.Select(orderItem => new OrderItemDto
                {
                    OfferId = orderItem.OfferId,
                    Quantity = orderItem.Quantity
                }).ToList()
            };
            AddDomainEvent(domainEvent);
        }

        public void ConfirmOrder()
        {
            ValidateConfirmOrder();

            OrderState = OrderState.InProgress;
            UpdatedAt = DateTime.UtcNow;

            var domainEvent = new OrderConfirmedDomainEvent
            {
                OrderId = Id,
                BuyerId = BuyerId,
                SellerId = SellerId
            };
            AddDomainEvent(domainEvent);
        }

        public void SetDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            ValidateDeliveryAddress(deliveryAddress);

            DeliveryAddress = deliveryAddress;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            ValidateDeliveryMethod(deliveryMethod);

            DeliveryMethod = deliveryMethod;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeOrderState(OrderState orderState)
        {
            ValidateChangeOrderState(orderState);

            if (orderState == OrderState) return;

            var previousOrderState = OrderState;

            OrderState = orderState;
            UpdatedAt = DateTime.UtcNow;

            var domainEvent = new OrderStatusChangedDomainEvent
            {
                OrderId = Id,
                BuyerId = BuyerId,
                PreviousState = previousOrderState,
                CurrentState = orderState
            };
            AddDomainEvent(domainEvent);
        }

        public decimal CalculateTotalPrice()
        {
            return DeliveryMethod != null
                ? TotalPrice + DeliveryMethod.Price
                : -1;
        }

        #region Validation

        private static void ValidateBuyerId(Guid buyerId)
        {
            var validator = new IdValidator();
            var result = validator.Validate(buyerId);
            if (!result.IsValid) throw new OrdersDomainException(nameof(buyerId));
        }

        private static void ValidateSellerId(Guid sellerId)
        {
            var validator = new IdValidator();
            var result = validator.Validate(sellerId);
            if (!result.IsValid) throw new OrdersDomainException(nameof(sellerId));
        }

        private static void ValidateOrderItems(IList<OrderItem> orderItems)
        {
            var hashSetCount = orderItems.Select(x => x.OfferId).ToHashSet().Count;
            if (hashSetCount != orderItems.Count)
                throw new OrdersDomainException($"Two order items cannot have the same {nameof(OrderItem.OfferId)}");
        }

        private void ValidateCancellation(OrderState orderState)
        {
            if (orderState == null || !orderState.IsCancelled())
                throw new OrdersDomainException("Provided order state is invalid");

            if (OrderState.IsCancelled())
                throw new OrdersDomainException("Order is already cancelled");

            if (OrderState == OrderState.Shipped)
                throw new OrdersDomainException("Order is already completed");
        }

        private void ValidateConfirmOrder()
        {
            if (OrderState != OrderState.Started)
                throw new OrdersDomainException("Invalid state transition");
            if (DeliveryMethod == null)
                throw new OrdersDomainException($"Order must have {nameof(DeliveryMethod)}");
            if (DeliveryAddress == null)
                throw new OrdersDomainException($"Order must have {nameof(DeliveryAddress)}");
        }

        private static void ValidateDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null)
                throw new OrdersDomainException($"'{nameof(deliveryAddress)}' cannot be null");
        }

        private static void ValidateDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            if (deliveryMethod == null)
                throw new OrdersDomainException($"'{nameof(deliveryMethod)}' cannot be null");
        }

        private void ValidateChangeOrderState(OrderState orderState)
        {
            if (IsCancelled)
                throw new OrdersDomainException("Order is already cancelled so its status cannot be changed");

            if (OrderState == OrderState.Started)
                throw new OrdersDomainException($"Order has {OrderState.Started.Name} state and cannot be changed manually");
        }

        #endregion
    }
}
