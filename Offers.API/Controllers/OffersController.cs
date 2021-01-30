using Common.Authentication;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.CreateOfferDraftOne;
using Offers.API.Application.Commands.EndOffer;
using Offers.API.Application.Commands.RemoveOffer;
using Offers.API.Application.Commands.UpdateOfferDraftOne;
using Offers.API.Application.Commands.UpdateOfferDraftTwo;
using Offers.API.Application.Dto;
using Offers.API.Application.Queries.GetMyOffer;
using Offers.API.Application.Queries.GetMyOffers;
using Offers.API.Application.Queries.GetOffer;
using Offers.API.Application.Types;
using System;
using System.Threading.Tasks;
using Offers.API.Application.Queries.GetActiveOffers;

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
        public async Task<Pagination<OfferDto>> GetActive([FromQuery] OfferFilter offerFilter)
        {
            var query = new GetActiveOffersQuery { OfferFilter = offerFilter };
            return await _mediator.Send(query);
        }

        [HttpGet("my")]
        [JwtAuthorize]
        public async Task<Pagination<OfferDto>> GetMyOffers([FromQuery] OfferFilter offerFilter)
        {
            var query = new GetMyOffersQuery { OfferFilter = offerFilter };
            return await _mediator.Send(query);
        }

        [HttpGet("{offerId}/my")]
        [JwtAuthorize]
        public async Task<OfferDto> GetMyOffers(string offerId)
        {
            var query = new GetMyOfferQuery { OfferId = offerId };
            return await _mediator.Send(query);
        }

        [HttpGet("{offerId}")]
        public async Task<OfferDto> GetById(string offerId)
        {
            var query = new GetOfferQuery { OfferId = offerId };
            return await _mediator.Send(query);
        }

        [HttpPost("draft/1")]
        [JwtAuthorize]
        public async Task<IActionResult> CreateDraftOne([FromForm] CreateOfferDraftOneCommand command)
        {
            var offerId = await _mediator.Send(command);
            return Ok(new { OfferId = offerId });
        }

        [HttpPut("draft/1")]
        [JwtAuthorize]
        public async Task<IActionResult> UpdateDraftOne([FromForm] UpdateOfferDraftOneCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("draft/2")]
        [JwtAuthorize]
        public async Task<IActionResult> UpdateDraftTwo([FromForm] UpdateOfferDraftTwoCommand command)
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

        [HttpDelete("")]
        [JwtAuthorize]
        public async Task<IActionResult> Remove(RemoveOfferCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
