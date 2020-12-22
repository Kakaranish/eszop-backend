using Common.Authentication;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.API.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Products.API.DataAccess.Repositories;
using Products.API.Domain;

namespace Products.API.Controllers
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
        public IEnumerable<Offer> GetAll()
        {
            return _offerRepository.GetAll();
        }
        
        [HttpPost("")]
        [JwtAuthorize]
        public async Task<IActionResult> Create(OfferAddedCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }
    }
}
