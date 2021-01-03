using Common.Domain;
using Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orders.API.Domain
{
    public class Order : EntityBase, IAggregateRoot, ITimeStamped
    {
        private List<OrderItem> _orderItems;
        
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public virtual IReadOnlyCollection<OrderItem> OrderItems => _orderItems ?? new List<OrderItem>();
        
        public Order()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        public void AddOrderItems(IList<OrderItem> orderItems)
        {
            ValidateOrderItems(orderItems);

            _orderItems ??= new List<OrderItem>();

            foreach (var orderItem in orderItems)
            {
                _orderItems.Add(orderItem);
            }
        }
        
        public void ValidateOrderItems(IList<OrderItem> orderItems)
        {
            var hashSetCount = orderItems.Select(x => x.OfferId).ToHashSet().Count;
            if (hashSetCount != orderItems.Count)
            {
                throw new OrdersDomainException($"Two order items cannot have the same {nameof(OrderItem.OfferId)}");
            }
        }
    }
}
