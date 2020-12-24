using System;
using System.Collections.Generic;
using Common.Types;
using Common.Types.Domain;

namespace Carts.API.Domain
{
    public class Cart : EntityBase, IAggregateRoot, ITimeStamped
    {
        public Guid UserId { get; set; }
        public virtual IList<CartItem> CartItems { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }

        public Cart(Guid userId)
        {
            ValidateUserId(userId);
            UserId = userId;
            
            CartItems = new List<CartItem>();
            
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        private static void ValidateUserId(Guid userId)
        {
            if (userId == Guid.Empty) throw new DomainException($"'{nameof(userId)}' is invalid id");
        }
    }
}
