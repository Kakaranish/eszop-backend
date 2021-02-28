using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Orders.API.Domain
{
    public class OrderState : IEquatable<OrderState>
    {
        public string Name { get; }

        public static readonly OrderState Started = new("STARTED");
        public static readonly OrderState InProgress = new("IN_PROGRESS");
        public static readonly OrderState InPreparation = new("IN_PREPARATION");
        public static readonly OrderState Shipped = new("SHIPPED");
        public static readonly OrderState Cancelled = new("CANCELLED");
        public static readonly OrderState CancelledByBuyer = new("CANCELLED_BY_BUYER");
        public static readonly OrderState CancelledBySeller = new("CANCELLED_BY_SELLER");

        private static readonly IReadOnlyList<OrderState> CancelOrderStates = new List<OrderState>
        {
            Cancelled,
            CancelledByBuyer,
            CancelledBySeller
        };

        private static IDictionary<string, OrderState> _orderStatesCache;

        protected OrderState(string name)
        {
            Name = name.ToUpperInvariant();
        }

        private OrderState()
        {
        }

        public bool IsCancelled()
        {
            return CancelOrderStates.Contains(this);
        }

        public static OrderState Parse(string orderState)
        {
            if (string.IsNullOrEmpty(orderState)) return null;

            EnsureOrderStatesCachePopulated();

            _orderStatesCache.TryGetValue(orderState.ToUpperInvariant(), out var matchingOrderState);
            return matchingOrderState;
        }

        public static bool IsValid(string orderState)
        {
            return Parse(orderState) != null;
        }

        private static void EnsureOrderStatesCachePopulated()
        {
            _orderStatesCache ??= typeof(OrderState)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<OrderState>()
                .ToDictionary(x => x.Name.ToUpperInvariant(), x => x);
        }

        public bool Equals(OrderState other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            
            return Equals((OrderState)obj);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        public static bool operator ==(OrderState obj1, OrderState obj2)
        {
            if (ReferenceEquals(obj1, obj2)) return true;
            if (ReferenceEquals(obj1, null)) return false;
            if (ReferenceEquals(obj2, null)) return false;

            return obj1.Equals(obj2);
        }

        public static bool operator !=(OrderState obj1, OrderState obj2)
        {
            return !(obj1 == obj2);
        }
    }
}
