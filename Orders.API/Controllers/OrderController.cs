using Common.Authentication;
using Common.Dto;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.API.Application.Commands.CancelOrder;
using Orders.API.Application.Commands.CreateOrder;
using Orders.API.Application.Commands.GetBankTransferDetails;
using Orders.API.Application.Dto;
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

        [HttpPost("")]
        [JwtAuthorize]
        public async Task<OrderCreatedDto> Create(CartDto cartDto)
        {
            var request = new CreateOrderCommand(cartDto);
            var orderId = await _mediator.Send(request);
            return new OrderCreatedDto { OrderId = orderId };
        }

        [HttpPost("cancel")]
        [JwtAuthorize]
        public async Task<IActionResult> Cancel(CancelOrderCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpGet("{orderId}/transfer/details")]
        [JwtAuthorize]
        public async Task<BankTransferDetailsDto> GetBankTransferDetails([FromRoute] GetBankTransferDetailsCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}
