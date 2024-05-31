using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Models;
using YachtMarinaAPI.RequestHelpers;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }


        [HttpPost("create")]
        [Authorize(Roles = "Właściciel")]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] CreateProductDto dto)
        {
            var id = await _service.Create(dto);

            return Created($"{id}", null);
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Właściciel, Bosman")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto dto)
        {
            await _service.Update(dto);

            return Ok();
        }

        [HttpDelete("{productId}/delete")]
        [Authorize(Roles = "Właściciel, Bosman")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            await _service.Delete(productId);

            return Ok();
        }


        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] ProductParams productParams)
        {
            var products = _service.GetAll(productParams);

            return Ok(products);
        }


        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById([FromRoute] int productId)
        {
            var product = await _service.GetById(productId);

            return Ok(product);
        }


        [HttpGet("getFilters")]
        public async Task<IActionResult> GetFilters()
        {
            var filters = _service.GetFilters();

            return Ok(filters);
        }

    }
}
