using Common.Dto;
using Offers.API.Application.Dto;
using Offers.API.Domain;
using System.Collections.Generic;
using System.Linq;

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
                Description = predefinedDeliveryMethod.Description,
                Price = predefinedDeliveryMethod.Price
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

        public static ImageInfoDto ToDto(this ImageInfo imageInfo)
        {
            if (imageInfo == null) return null;

            return new ImageInfoDto
            {
                Id = imageInfo.Id,
                Uri = imageInfo.Uri,
                SortId = imageInfo.SortId,
                IsMain = imageInfo.IsMain
            };
        }

        public static OfferFullViewDto ToOfferFullViewDto(this Offer offer)
        {
            if (offer == null) return null;

            return new OfferFullViewDto
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
                DeliveryMethods = offer.DeliveryMethods?.ToList() ?? new List<DeliveryMethod>(),
                Images = offer.Images?.Select(x => x.ToDto()).ToList() ?? new List<ImageInfoDto>(),
                KeyValueInfos = offer.KeyValueInfos?.ToList() ?? new List<KeyValueInfo>(),
                BankAccountNumber = offer.BankAccountNumber
            };
        }

        public static OfferViewDto ToOfferViewDto(this Offer offer)
        {
            if (offer == null) return null;

            return new OfferViewDto
            {
                Id = offer.Id,
                UpdatedAt = offer.UpdatedAt,
                UserEndedAt = offer.UserEndedAt,
                EndsAt = offer.EndsAt,
                PublishedAt = offer.PublishedAt,
                OwnerId = offer.OwnerId,
                Name = offer.Name,
                Description = offer.Description,
                Price = offer.Price,
                AvailableStock = offer.AvailableStock,
                TotalStock = offer.TotalStock,
                Category = offer.Category.ToDto(),
                DeliveryMethods = offer.DeliveryMethods?.ToList() ?? new List<DeliveryMethod>(),
                Images = offer.Images?.Select(x => x.ToDto()).ToList() ?? new List<ImageInfoDto>(),
                KeyValueInfos = offer.KeyValueInfos?.ToList() ?? new List<KeyValueInfo>()
            };
        }

        public static OfferListPreviewDto ToOfferListPreviewDto(this Offer offer)
        {
            if (offer == null) return null;

            return new OfferListPreviewDto
            {
                Id = offer.Id,
                EndsAt = offer.EndsAt,
                PublishedAt = offer.PublishedAt,
                Name = offer.Name,
                Price = offer.Price,
                AvailableStock = offer.AvailableStock,
                Category = offer.Category.ToDto(),
                MainImage = offer.Images?.FirstOrDefault(x => x.IsMain).ToDto()
            };
        }

        public static DeliveryMethodDto ToDto(this DeliveryMethod deliveryMethod)
        {
            if (deliveryMethod == null) return null;

            return new DeliveryMethodDto
            {
                Name = deliveryMethod.Name,
                Price = deliveryMethod.Price
            };
        }
    }
}
