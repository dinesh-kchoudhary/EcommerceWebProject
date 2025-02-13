using BrainHubTest.Model;
using BrainHubTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrainHubTest.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService) { _productService = productService; }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _productService.GetAllAsync(search, page, pageSize));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromForm] ProductRequest request, [FromForm] List<IFormFile> images)
        {
            return Ok(await _productService.AddProductAsync(request, images));
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] ProductRequest request)
        {
            return Ok(await _productService.EditProductAsync(id, request));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
