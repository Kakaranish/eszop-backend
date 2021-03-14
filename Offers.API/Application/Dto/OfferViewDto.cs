using Offers.API.Domain;
using System;
using System.Collections.Generic;

namespace Offers.API.Application.Dto
{
    public class OfferViewDto
    {
        public Guid Id { get; init; }

        public DateTime UpdatedAt { get; init; }
        public DateTime EndsAt { get; init; }
        public DateTime? UserEndedAt { get; init; }
        public DateTime? PublishedAt { get; init; }
        public bool IsActive { get; init; }

        public Guid OwnerId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int AvailableStock { get; init; }
        public int TotalStock { get; init; }

        public CategoryDto Category { get; init; }
        public List<DeliveryMethod> DeliveryMethods { get; init; }
        public List<ImageInfoDto> Images { get; init; }
        public List<KeyValueInfo> KeyValueInfos { get; init; }
    }
}
