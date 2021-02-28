using Common.Authentication;
using Common.Dto;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.API.Application.Commands.CancelOrder;
using Orders.API.Application.Commands.ConfirmOrder;
using Orders.API.Application.Commands.CreateOrder;
using Orders.API.Application.Commands.GetBankTransferDetails;
using Orders.API.Application.Commands.UpdateDeliveryInfo;
using Orders.API.Application.Dto;
using Orders.API.Application.Queries.GetAvailableDeliveryMethodsForOrder;
using Orders.API.Application.Queries.GetDeliveryInfo;
using Orders.API.Application.Queries.GetOrders;
using Orders.API.Application.Queries.GetOrderSummary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryMethodDto = Common.Dto.DeliveryMethodDto;

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

        [HttpGet("user")]
        [JwtAuthorize]
        public async Task<Pagination<OrderPreviewDto>> GetAllByUserId([FromQuery] BasicPaginationFilter filter)
        {
            var query = new GetOrdersQuery { Filter = filter };
            return await _mediator.Send(query);
        }

        [HttpGet("{orderId}")]
        [JwtAuthorize]
        public async Task<OrderDto> GetOrderSummary([FromRoute] GetOrderSummaryQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("{orderId}/available-delivery-methods")]
        [JwtAuthorize]
        public async Task<IList<DeliveryMethodDto>> GetDeliveryMethodsForOrder(
            [FromRoute] GetAvailableDeliveryMethodsForOrderQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("{orderId}/delivery-info")]
        [JwtAuthorize]
        public async Task<DeliveryInfoDto> GetDeliveryInfo([FromRoute] GetDeliveryInfoQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("{orderId}/transfer/details")]
        [JwtAuthorize]
        public async Task<BankTransferDetailsDto> GetBankTransferDetails([FromRoute] GetBankTransferDetailsCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("{orderId}/delivery-info")]
        [JwtAuthorize]
        public async Task<IActionResult> UpdateDeliveryAddress(string orderId, UpdateDeliveryInfoCommand request)
        {
            request.OrderId = orderId;
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("")]
        [JwtAuthorize]
        public async Task<OrderCreatedDto> Create(CreateOrderCartDto createOrderCartDto)
        {
            var request = new CreateOrderCommand(createOrderCartDto);
            var orderId = await _mediator.Send(request);
            return new OrderCreatedDto { OrderId = orderId };
        }

        [HttpPost("{orderId}/cancel")]
        [JwtAuthorize]
        public async Task<IActionResult> Cancel([FromRoute] CancelOrderCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("{orderId}/confirm")]
        [JwtAuthorize]
        public async Task<IActionResult> Confirm([FromRoute] ConfirmOrderCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
