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
        public Guid UserId { get; private set; }
        public virtual IList<CartItem> CartItems { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        [NotMapped] public bool IsEmpty => (CartItems?.Count ?? 0) == 0;

        public Cart(Guid userId)
        {
            ValidateUserId(userId);
            UserId = userId;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        public void AddCartItem(CartItem cartItem)
        {
            CartItems ??= new List<CartItem>();

            if (CartItems.Any(item => item.SellerId != cartItem.SellerId))
            {
                throw new CartsDomainException("Offer from other seller is already in cart");
            }
            if (CartItems.Any(item => item.OfferId == cartItem.OfferId))
            {
                throw new CartsDomainException($"Offer {cartItem.OfferId} is already in cart");
            }

            CartItems.Add(cartItem);
        }

        public void ClearCartItems()
        {
            CartItems?.Clear();
        }

        private static void ValidateUserId(Guid userId)
        {
            var idValidator = new IdValidator();
            var result = idValidator.Validate(userId);
            if (!result.IsValid) throw new CartsDomainException($"'{nameof(userId)}' is invalid id");
        }
    }
}
