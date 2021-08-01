using Common.Domain.Types;
using Common.Utilities.Authentication;
using Common.Utilities.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.API.Application.Commands.CancelOrder;
using Orders.API.Application.Commands.ChangeOrderState;
using Orders.API.Application.Commands.ConfirmOrder;
using Orders.API.Application.Commands.GetBankTransferDetails;
using Orders.API.Application.Commands.UpdateDeliveryInfo;
using Orders.API.Application.Dto;
using Orders.API.Application.Queries.GetAvailableDeliveryMethodsForOrder;
using Orders.API.Application.Queries.GetDeliveryInfo;
using Orders.API.Application.Queries.GetOrdersAsBuyer;
using Orders.API.Application.Queries.GetOrdersAsSeller;
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

        [HttpGet("buyer")]
        [JwtAuthorize]
        public async Task<Pagination<OrderPreviewDto>> GetAllByBuyerId([FromQuery] BasicPaginationFilter filter)
        {
            var query = new GetOrdersAsBuyerQuery { Filter = filter };
            return await _mediator.Send(query);
        }

        [HttpGet("seller")]
        [JwtAuthorize]
        public async Task<Pagination<OrderPreviewDto>> GetAllBySellerId([FromQuery] BasicPaginationFilter filter)
        {
            var query = new GetOrdersAsSellerQuery { Filter = filter };
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
        public async Task<IActionResult> UpdateDeliveryAddress([FromRoute] string orderId, UpdateDeliveryInfoCommand request)
        {
            request.OrderId = orderId;
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("{orderId}/cancel")]
        [JwtAuthorize]
        public async Task<IActionResult> Cancel([FromRoute] CancelOrderCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPut("{orderId}/state")]
        [JwtAuthorize]
        public async Task<IActionResult> ChangeState([FromRoute] string orderId, ChangeOrderStateCommand command)
        {
            command.OrderId = orderId;
            await _mediator.Send(command);
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
