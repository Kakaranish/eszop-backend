using Common.Types.Domain;
using System;

namespace Orders.API.Domain
{
    // TODO:
    public class OrderItem : EntityBase, IAggregateRoot
    {
        public Guid OfferId { get; set; }
        public string OfferName { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
    }
}
