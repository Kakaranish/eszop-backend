using System;
using System.Collections.Generic;
using Common.Types.Domain;

namespace Carts.API.Domain
{
    public class Cart : EntityBase, IAggregateRoot
    {
        public Guid UserId { get; set; }
        public virtual IList<CartItem> CartItems { get; set; }
    }
}
