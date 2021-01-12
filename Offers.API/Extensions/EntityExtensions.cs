using System.Collections.Generic;
using System.Linq;
using Offers.API.Application.Dto;
using Offers.API.Domain;

namespace Offers.API.Extensions
{
    public static class EntityExtensions
    {
        public static PredefinedDeliveryMethodDto ToDto(this PredefinedDeliveryMethod predefinedDeliveryMethod)
        {
            if (predefinedDeliveryMethod == null) return null;

            return new PredefinedDeliveryMethodDto
            {
                Id = predefinedDeliveryMethod.Id,
                Name = predefinedDeliveryMethod.Name,
                Description = predefinedDeliveryMethod.Description
            };
        }

        public static CategoryDto ToDto(this Category category)
        {
            if (category == null) return null;
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static OfferDto ToDto(this Offer offer)
        {
            if (offer == null) return null;

            return new OfferDto
            {
                Id = offer.Id,
                CreatedAt = offer.CreatedAt,
                UpdatedAt = offer.UpdatedAt,
                UserEndedAt = offer.UserEndedAt,
                EndsAt = offer.EndsAt,
                RemovedAt = offer.RemovedAt,
                PublishedAt = offer.PublishedAt,
                OwnerId = offer.OwnerId,
                Name = offer.Name,
                Description = offer.Description,
                Price = offer.Price,
                AvailableStock = offer.AvailableStock,
                TotalStock = offer.TotalStock,
                Category = offer.Category.ToDto(),
                DeliveryMethods = offer.DeliveryMethods?.ToList() ?? new List<DeliveryMethod>()
            };
        }
    }
}
