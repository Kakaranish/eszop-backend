using Common.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Common.Domain;
using Common.Validators;

namespace Carts.API.Domain
{
    public class Cart : EntityBase, IAggregateRoot, ITimeStamped
    {
        private List<CartItem> _cartItems;

        public Guid UserId { get; private set; }
        public virtual IReadOnlyCollection<CartItem> CartItems => _cartItems ?? new List<CartItem>();
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        [NotMapped] public bool IsEmpty => (_cartItems?.Count ?? 0) == 0;

        public Cart(Guid userId)
        {
            ValidateUserId(userId);
            UserId = userId;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        public void AddCartItem(CartItem cartItem)
        {
            _cartItems ??= new List<CartItem>();

            if (_cartItems.Any(item => item.SellerId != cartItem.SellerId))
            {
                throw new CartsDomainException("Offer from other seller is already in cart");
            }
            if (_cartItems.Any(item => item.OfferId == cartItem.OfferId))
            {
                throw new CartsDomainException($"Offer {cartItem.OfferId} is already in cart");
            }

            _cartItems.Add(cartItem);
        }

        public void ClearCartItems()
        {
            _cartItems?.Clear();
        }

        private static void ValidateUserId(Guid userId)
        {
            var idValidator = new IdValidator();
            var result = idValidator.Validate(userId);
            if (!result.IsValid) throw new CartsDomainException($"'{nameof(userId)}' is invalid id");
        }
    }
}
