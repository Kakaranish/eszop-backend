using Carts.API.DataAccess.Repositories;
using Common.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carts.API.Domain;

namespace Carts.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/")]
    public class CartController : BaseController
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        // TEMP Endpoint
        [HttpGet("all")]
        public async Task<IList<Cart>> GetAll()
        {
            return await _cartRepository.GetAllAsync();
        }

        // TEMP Endpoint
        [HttpPost("add")]
        public async Task<IActionResult> Add()
        {
            var cartItems = new List<CartItem>
            {
                new()
                {
                    TotalPrice = 200m,
                    PricePerItem = 200m,
                    Quantity = 1,
                    OfferId = Guid.NewGuid(),
                    OfferName = $"Offer name {DateTime.UtcNow.ToLongTimeString()}",
                    SellerEmail = $"Seller email {DateTime.UtcNow.ToLongTimeString()}",
                    OfferPhotoUrl = $"Offer photo url {DateTime.UtcNow.ToLongTimeString()}",
                    SellerId = Guid.NewGuid()
                }
            };

            var cart = new Cart
            {
                UserId = Guid.NewGuid(),
                CartItems = cartItems
            };

            await _cartRepository.AddAsync(cart);

            return Ok();
        }
    }
}
