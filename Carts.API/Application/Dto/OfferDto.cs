using System;
using System.Collections.Generic;

namespace Carts.API.Application.Dto
{
    public class OfferDto
    {
        public Guid Id { get; init; }
        public Guid OwnerId { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public int AvailableStock { get; init; }
        public List<ImageDto> Images { get; init; }
    }
}
