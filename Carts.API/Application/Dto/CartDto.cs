using System;
using System.Collections.Generic;

namespace Carts.API.Application.Dto
{
    public class CartDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public IList<CartItemDto> CartItems { get; init; }
    }
}
