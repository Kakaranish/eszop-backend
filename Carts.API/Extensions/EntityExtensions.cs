using Carts.API.Application.Dto;
using Carts.API.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Carts.API.Extensions
{
    public static class EntityExtensions
    {
        public static CartDto ToDto(this Cart cart)
        {
            if (cart == null) return null;

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CartItems = cart.CartItems?.Select(x => x.ToDto()).ToList() ?? new List<CartItemDto>()
            };
        }

        public static CartItemDto ToDto(this CartItem cartItem)
        {
            if (cartItem == null) return null;

            return new CartItemDto
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                CartOwnerId = cartItem.CartOwnerId,
                OfferId = cartItem.OfferId,
                SellerId = cartItem.SellerId,
                OfferName = cartItem.OfferName,
                PricePerItem = cartItem.PricePerItem,
                Quantity = cartItem.Quantity,
                AvailableStock = cartItem.AvailableStock,
                ImageUri = cartItem.ImageUri
            };
        }
    }
}
