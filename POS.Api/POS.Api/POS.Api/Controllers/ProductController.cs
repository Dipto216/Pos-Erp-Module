using Microsoft.AspNetCore.Mvc;
using POS.Api.Data;
using POS.Api.Models;

namespace POS.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Product create failed", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context.Products.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to get products", error = ex.Message });
            }
        }
    }
}
