namespace Common.Types
{
    public static class NotificationCodes
    {
        public static readonly string CartItemChanged = nameof(CartItemChanged);

        public static readonly string CartItemRemoved = nameof(CartItemRemoved);

        public static readonly string CartItemBecameUnavailable = nameof(CartItemBecameUnavailable);

        public static readonly string OfferBecameUnavailable = nameof(OfferBecameUnavailable);

        public static readonly string OrderStateChanged = nameof(OrderStateChanged);

        public static readonly string OrderStarted = nameof(OrderStarted);

        public static readonly string OrderConfirmed = nameof(OrderConfirmed);

        public static readonly string OrderCancelled = nameof(OrderCancelled);
    }
}
