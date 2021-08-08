using Common.Dto;
using Orders.API.Application.Dto;
using Orders.Domain.Aggregates.DeliveryAddressAggregate;
using Orders.Domain.Aggregates.OrderAggregate;
using Orders.Domain.Aggregates.OrderItemAggregate;
using System.Linq;
using OrderItemDto = Orders.API.Application.Dto.OrderItemDto;

namespace Orders.API.Extensions
{
    public static class EntityExtensions
    {
        public static OrderPreviewDto ToPreviewDto(this Order order)
        {
            if (order == null) return null;

            return new OrderPreviewDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                SellerId = order.SellerId,
                OrderState = order.OrderState.Name,
                TotalPrice = order.OrderItems.Sum(x => x.TotalPrice),
                OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList()
            };
        }

        public static OrderDto ToDto(this Order order)
        {
            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                SellerId = order.SellerId,
                BuyerId = order.Buyer.Id,
                OrderState = order.OrderState.Name,
                TotalPrice = order.OrderItems.Sum(x => x.TotalPrice),
                OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
                DeliveryAddress = order.Buyer.DeliveryAddress.ToDto(),
                DeliveryMethod = order.DeliveryMethod.ToDto()
            };
        }

        public static OrderItemDto ToDto(this OrderItem orderItem)
        {
            if (orderItem == null) return null;

            return new OrderItemDto
            {
                Id = orderItem.Id,
                Quantity = orderItem.Quantity,
                OfferId = orderItem.OfferDetails.Id,
                OfferName = orderItem.OfferDetails.Name,
                PricePerItem = orderItem.OfferDetails.PricePerItem,
                ImageUri = orderItem.OfferDetails.ImageUri
            };
        }

        public static DeliveryAddressDto ToDto(this DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null) return null;

            return new DeliveryAddressDto
            {
                Id = deliveryAddress.Id,
                FirstName = deliveryAddress.FirstName,
                LastName = deliveryAddress.LastName,
                PhoneNumber = deliveryAddress.PhoneNumber,
                Country = deliveryAddress.Country,
                City = deliveryAddress.City,
                Street = deliveryAddress.Street,
                ZipCode = deliveryAddress.ZipCode
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
