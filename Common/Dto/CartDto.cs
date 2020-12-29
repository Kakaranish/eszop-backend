using System;
using System.Collections.Generic;

namespace Common.Dto
{
    public class CartDto
    {
        public Guid UserId { get; init; }
        public IList<CartItemDto> CartItems { get; init; }
    }
}
