using Common.Authentication;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.CreateOffer;
using Offers.API.Application.Commands.EndOffer;
using Offers.API.Application.Commands.RemoveOffer;
using Offers.API.Application.Commands.UpdateOffer;
using Offers.API.Application.Dto;
using Offers.API.Application.Queries.GetAllOffers;
using Offers.API.Application.Queries.GetFilteredOffers;
using Offers.API.Application.Queries.GetOffer;
using Offers.API.Application.Queries.GetUserOffers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class OffersController : BaseController
    {
        private readonly IMediator _mediator;

        public OffersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("")]
        public async Task<IList<OfferDto>> GetAll()
        {
            var query = new GetAllOffersQuery();
            return await _mediator.Send(query);
        }

        [HttpGet("filtered")]
        public async Task<Pagination<OfferDto>> GetFiltered([FromQuery] GetFilteredOffersQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("{offerId}")]
        public async Task<OfferDto> GetById(string offerId)
        {
            var query = new GetOfferQuery { OfferId = offerId };
            return await _mediator.Send(query);
        }

        [HttpGet("user")]
        [JwtAuthorize]
        public async Task<IList<OfferDto>> GetByUser()
        {
            var query = new GetUserOffersQuery();
            return await _mediator.Send(query);
        }

        [HttpPost("")]
        [JwtAuthorize]
        public async Task<IActionResult> Create([FromForm] CreateOfferCommand command)
        {
            var offerId = await _mediator.Send(command);
            return Ok(new { OfferId = offerId });
        }

        [HttpPut("")]
        [JwtAuthorize]
        public async Task<IActionResult> Update(UpdateOfferCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("end")]
        [JwtAuthorize]
        public async Task<IActionResult> End(EndOfferCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [JwtAuthorize]
        public async Task<IActionResult> Remove(RemoveOfferCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
