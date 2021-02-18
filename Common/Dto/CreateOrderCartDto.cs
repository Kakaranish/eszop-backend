using System;
using System.Collections.Generic;

namespace Common.Dto
{
    public class CreateOrderCartDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public Guid SellerId { get; init; }
        public IList<CreateOrderCartItemDto> CartItems { get; init; }
    }
}
