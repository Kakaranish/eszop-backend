using Carts.API.Domain;
using Common.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Carts.API
{
    public static class Extensions
    {
        public static CartDto ToDto(this Cart cart)
        {
            var cartItemDtos = cart.CartItems?.Select(item => new CartItemDto
            {
                Id = item.Id,
                CartId = item.CartId,
                OfferId = item.OfferId,
                SellerId = item.SellerId,
                OfferName = item.OfferName,
                Quantity = item.Quantity,
                PricePerItem = item.PricePerItem
            }).ToList() ?? new List<CartItemDto>();

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CartItems = cartItemDtos
            };
        }
    }
}
