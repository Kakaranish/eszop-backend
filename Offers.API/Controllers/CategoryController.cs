using Common.Authentication;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.CreateCategory;
using Offers.API.Application.Queries.GetCategories;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]/")]
    public class CategoryController : BaseController
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("")]
        public async Task<IList<Category>> GetAll()
        {
            var request = new GetCategoriesQuery();
            var categories = await _mediator.Send(request);
            return categories;
        }

        [HttpPost("")]
        [JwtAuthorize("Admin")]
        public async Task<IActionResult> Create(CreateCategoryCommand request)
        {
            var categoryId = await _mediator.Send(request);
            return Ok(new { CategoryId = categoryId });
        }
    }
}
