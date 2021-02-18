using System.Linq;
using Orders.API.Application.Dto;
using Orders.API.Domain;

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
                OrderState = order.OrderState,
                TotalPrice = order.OrderItems.Sum(x => x.TotalPrice),
                OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList()
            };
        }
        
        public static OrderItemDto ToDto(this OrderItem orderItem)
        {
            if (orderItem == null) return null;

            return new OrderItemDto
            {
                Id = orderItem.Id,
                OfferId = orderItem.OfferId,
                OfferName = orderItem.OfferName,
                PricePerItem = orderItem.PricePerItem,
                Quantity = orderItem.Quantity
            };
        }
    }
}
