using System;
using Common.Types;
using Common.Types.Domain;

namespace Offers.API.Domain
{
    public class Offer : EntityBase, IAggregateRoot
    {
        // TEMP public setters
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndsAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
