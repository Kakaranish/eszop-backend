using System.Collections.Generic;
using Common.Domain;

namespace Orders.API.Domain
{
    // TODO:
    public class Order : EntityBase, IAggregateRoot
    {
        public virtual IList<OrderItem> OrderItems { get; set; }
    }
}
