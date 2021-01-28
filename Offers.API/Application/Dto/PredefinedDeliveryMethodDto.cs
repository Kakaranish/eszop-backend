using System;

namespace Offers.API.Application.Dto
{
    public class PredefinedDeliveryMethodDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal? Price { get; init; }
    }
}
