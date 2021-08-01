using Common.Utilities.Authentication;
using Common.Utilities.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.CreateCategory;
using Offers.API.Application.Commands.UpdateCategory;
using Offers.API.Application.Queries.GetCategories;
using Offers.API.Application.Queries.GetCategory;
using Offers.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/")]
    public class CategoriesController : BaseController
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("")]
        public async Task<IList<CategoryDto>> GetAll()
        {
            var request = new GetCategoriesQuery();
            var categories = await _mediator.Send(request);
            return categories;
        }

        [HttpGet("{categoryId}")]
        public async Task<CategoryDto> GetById([FromRoute] GetCategoryQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("")]
        [JwtAuthorize("SuperAdmin")]
        public async Task<IActionResult> Create(CreateCategoryCommand request)
        {
            var categoryId = await _mediator.Send(request);
            return Ok(new { CategoryId = categoryId });
        }

        [HttpPut("{categoryId}")]
        [JwtAuthorize("SuperAdmin")]
        public async Task<IActionResult> Update(string categoryId, [FromBody] UpdateCategoryCommand request)
        {
            request.CategoryId = categoryId;
            await _mediator.Send(request);
            return Ok();
        }
    }
}
