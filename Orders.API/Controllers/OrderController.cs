using Common.Authentication;
using Common.Dto;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.API.Application.Commands.CreateOrder;
using System;
using System.Threading.Tasks;

namespace Orders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class OrderController : BaseController
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("create")]
        [JwtAuthorize]
        public async Task<OrderCreatedDto> CreateOrder(CartDto cartDto)
        {
            var request = new CreateOrderCommand(cartDto);
            var orderId = await _mediator.Send(request);
            return new OrderCreatedDto { OrderId = orderId };
        }
    }
}
