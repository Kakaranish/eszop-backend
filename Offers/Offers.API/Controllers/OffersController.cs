﻿using Common.Domain.Types;
using Common.Utilities.Authentication;
using Common.Utilities.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.EndOffer;
using Offers.API.Application.Commands.RemoveOffer;
using Offers.API.Application.Commands.UpdateActiveOffer;
using Offers.API.Application.Queries.GetActiveOffers;
using Offers.API.Application.Queries.GetMyOffer;
using Offers.API.Application.Queries.GetMyOffers;
using Offers.API.Application.Queries.GetOffer;
using Offers.API.Application.Queries.GetSellerOffers;
using Offers.Domain.Repositories.Types;
using Offers.Infrastructure.Dto;
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
        public async Task<Pagination<OfferListPreviewDto>> GetSellerOffers(
            [FromRoute] string sellerId, [FromQuery] OfferFilter offerFilter)
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

        [HttpPost("{offerId}/end")]
        [JwtAuthorize]
        public async Task<IActionResult> End([FromRoute] EndOfferCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{offerId}")]
        [JwtAuthorize]
        public async Task<IActionResult> UpdateActiveOffer([FromRoute] string offerId,
            [FromForm] UpdateActiveOfferCommand command)
        {
            command.OfferId = offerId;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{offerId}")]
        [JwtAuthorize]
        public async Task<IActionResult> Remove([FromRoute] RemoveOfferCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
