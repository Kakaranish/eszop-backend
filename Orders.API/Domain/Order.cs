using Common.Types.Domain;
using System.Collections.Generic;

namespace Orders.API.Domain
{
    // TODO:
    public class Order : EntityBase, IAggregateRoot
    {
        public virtual IList<OrderItem> OrderItems { get; set; }
    }
}
