using Microsoft.AspNetCore.Mvc;
using Products.API.DataAccess.Repositories;
using Products.API.Domain;
using Products.API.Dto;
using System;
using System.Threading.Tasks;

namespace Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpGet("")]
        public IActionResult GetAllProducts()
        {
            return Ok(_productRepository.GetAll());
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price
            };

            await _productRepository.AddAsync(product);

            return Ok();
        }
    }
}
