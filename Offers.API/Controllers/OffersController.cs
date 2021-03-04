using Common.Authentication;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.EndOffer;
using Offers.API.Application.Commands.PublishOffer;
using Offers.API.Application.Commands.RemoveOffer;
using Offers.API.Application.Dto;
using Offers.API.Application.Queries.GetActiveOffers;
using Offers.API.Application.Queries.GetMyOffer;
using Offers.API.Application.Queries.GetMyOffers;
using Offers.API.Application.Queries.GetOffer;
using Offers.API.Application.Queries.GetSellerOffers;
using Offers.API.Application.Types;
using System;
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
        public async Task<Pagination<OfferListPreviewDto>> GetActive([FromQuery] OfferFilter offerFilter)
        {
            var query = new GetActiveOffersQuery { OfferFilter = offerFilter };
            return await _mediator.Send(query);
        }

        [HttpGet("my")]
        [JwtAuthorize]
        public async Task<Pagination<OfferListPreviewDto>> GetMyOffers([FromQuery] OfferFilter offerFilter)
        {
            var query = new GetMyOffersQuery { OfferFilter = offerFilter };
            return await _mediator.Send(query);
        }

        [HttpGet("seller/{sellerId}")]
        public async Task<Pagination<OfferListPreviewDto>> GetSellerOffers([FromRoute] string sellerId, [FromQuery] OfferFilter offerFilter)
        {
            var query = new GetSellerOffersQuery
            {
                SellerId = sellerId,
                OfferFilter = offerFilter
            };
            return await _mediator.Send(query);
        }

        [HttpGet("{offerId}/my")]
        [JwtAuthorize]
        public async Task<OfferFullViewDto> GetMyOffer([FromRoute] GetMyOfferQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("{offerId}")]
        public async Task<OfferViewDto> GetById(string offerId)
        {
            var query = new GetOfferQuery { OfferId = offerId };
            return await _mediator.Send(query);
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

        [HttpPost("{offerId}/publish")]
        [JwtAuthorize]
        public async Task<IActionResult> Publish([FromRoute] PublishOfferCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
