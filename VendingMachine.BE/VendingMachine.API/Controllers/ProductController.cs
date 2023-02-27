using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VendingMachine.API.Models;
using VendingMachine.BLL.Interfaces;
using VendingMachine.DAL.Entities;

namespace VendingMachine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] Guid id)
        {
            var result = await _productService.GetProduct(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _productService.GetAllProducts();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Add([FromBody] ProductRequest product)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            await _productService.AddProduct(product, userId);

            return Ok();
        }

        [HttpPut("update")]
        [Authorize(Roles = "Seller")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody] DAL.Entities.Product product)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _productService.UpdateProduct(product, userId);

            return Ok();
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Seller")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove([FromQuery] Guid productId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _productService.DeleteProduct(productId, userId);

            return Ok();
        }
    }
}
