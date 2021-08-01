using System;

namespace Common.Dto
{
    public class OrderItemDto
    {
        public Guid OfferId { get; init; }
        public int Quantity { get; init; }
    }
}
