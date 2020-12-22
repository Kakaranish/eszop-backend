using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Authentication;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.CreateOffer;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;

namespace Offers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class OffersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IOfferRepository _offerRepository;

        public OffersController(IMediator mediator, IOfferRepository offerRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        [HttpGet("")]
        public async Task<IList<Offer>> GetAll()
        {
            return await _offerRepository.GetAllAsync();
        }
        
        [HttpPost("")]
        [JwtAuthorize]
        public async Task<IActionResult> Create(CreateOfferCommand command)
        {
            var offerId = await _mediator.Send(command);

            return Ok(new { OfferId = offerId });
        }
    }
}
