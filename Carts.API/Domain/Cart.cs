using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Types;
using Common.Types.Domain;

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
            
            // TODO: Validate if not duplicated
            
            CartItems.Add(cartItem);
        }
        
        public void ClearCartItems()
        {
            CartItems?.Clear();
        }

        private static void ValidateUserId(Guid userId)
        {
            if (userId == Guid.Empty) throw new DomainException($"'{nameof(userId)}' is invalid id");
        }
    }
}
