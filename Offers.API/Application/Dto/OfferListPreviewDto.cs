using System;

namespace Offers.API.Application.Dto
{
    public class OfferListPreviewDto
    {
        public Guid Id { get; init; }

        public DateTime EndsAt { get; init; }
        public DateTime? PublishedAt { get; init; }
        public bool IsActive { get; init; }

        public string Name { get; init; }
        public decimal Price { get; init; }
        public int AvailableStock { get; init; }

        public CategoryDto Category { get; init; }
        public ImageInfoDto MainImage { get; init; }
    }
}
