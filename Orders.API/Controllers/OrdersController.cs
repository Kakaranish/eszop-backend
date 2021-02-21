using Common.Authentication;
using Common.Dto;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.API.Application.Commands.CancelOrder;
using Orders.API.Application.Commands.CreateOrder;
using Orders.API.Application.Commands.GetBankTransferDetails;
using Orders.API.Application.Commands.UpdateDeliveryAddress;
using Orders.API.Application.Dto;
using Orders.API.Application.Queries;
using System;
using System.Threading.Tasks;

namespace Orders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class OrdersController : BaseController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("")]
        [JwtAuthorize]
        public async Task<Pagination<OrderPreviewDto>> GetAllByUserId([FromQuery] BasicPaginationFilter filter)
        {
            var query = new GetOrdersQuery { Filter = filter };
            return await _mediator.Send(query);
        }

        [HttpPost("")]
        [JwtAuthorize]
        public async Task<OrderCreatedDto> Create(CreateOrderCartDto createOrderCartDto)
        {
            var request = new CreateOrderCommand(createOrderCartDto);
            var orderId = await _mediator.Send(request);
            return new OrderCreatedDto { OrderId = orderId };
        }

        [HttpPut("delivery")]
        [JwtAuthorize]
        public async Task<IActionResult> UpdateDeliveryAddress(UpdateDeliveryAddressCommand request)
        {
            await _mediator.Send(request);
            return Ok();
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
