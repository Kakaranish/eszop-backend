using Common.Types;
using Identity.API.Application.Queries.GetSellerInfo;
using Identity.API.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/seller")]
    public class SellerInfoController : BaseController
    {
        private readonly IMediator _mediator;

        public SellerInfoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{sellerId}")]
        public async Task<SellerInfo> Get([FromRoute] GetSellerInfoQuery request)
        {
            return await _mediator.Send(request);
        }
    }
}
