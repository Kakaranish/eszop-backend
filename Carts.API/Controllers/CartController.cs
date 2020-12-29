using Carts.API.Application.Commands.AddToCart;
using Carts.API.Application.Commands.ClearCart;
using Carts.API.Application.Commands.FinalizeCart;
using Carts.API.Application.Queries.GetOrCreateCart;
using Carts.API.Domain;
using Common.Authentication;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Common.Dto;

namespace Carts.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/")]
    public class CartController : BaseController
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("")]
        [JwtAuthorize]
        public async Task<CartDto> GetCart()
        {
            return await _mediator.Send(new GetOrCreateCartQuery());
        }

        [HttpPost("add")]
        [JwtAuthorize]
        public async Task<IActionResult> AddToCart(AddToCartCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("clear")]
        [JwtAuthorize]
        public async Task<IActionResult> ClearCart()
        {
            await _mediator.Send(new ClearCartCommand());
            return Ok();
        }

        [HttpPost("finalize")]
        [JwtAuthorize]
        public async Task<IActionResult> FinalizeCart()
        {
            var orderId = await _mediator.Send(new FinalizeCartCommand());
            return Ok(new { OrderId = orderId });
        }
    }
}
