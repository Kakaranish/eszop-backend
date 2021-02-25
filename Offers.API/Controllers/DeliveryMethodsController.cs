using Common.Authentication;
using Common.Dto;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Queries.GetDeliveryMethodsForOffers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.API.Controllers
{
    [ApiController]
    [Route("/api/delivery-methods/")]
    public class DeliveryMethodsController : BaseController
    {
        private readonly IMediator _mediator;

        public DeliveryMethodsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("offers")]
        [JwtAuthorize]
        public Task<IList<DeliveryMethodDto>> GetForMultipleOffers([FromQuery] GetDeliveryMethodsForOffersQuery query)
        {
            return _mediator.Send(query);
        }
    }
}
