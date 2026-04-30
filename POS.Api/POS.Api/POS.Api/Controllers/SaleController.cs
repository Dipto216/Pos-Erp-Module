using Microsoft.AspNetCore.Mvc;
using POS.Api.Data;
using POS.Api.Models;

namespace POS.Api.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SaleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SaleController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> Create(Sale sale)
        {
            try
            {
                var product = await _context.Products.FindAsync(sale.ProductId);
                if (product == null)
                    return NotFound(new { message = "Product not found" });

                if (product.Stock < sale.Quantity)
                    return BadRequest(new { message = "Stock is not enough" });

                var today = DateTime.Now.ToString("yyyy-MM-dd");
                var todayCount = _context.Sales
                    .Where(s => s.SaleId != null && s.SaleId.StartsWith(today))
                    .Count();

                sale.SaleId = $"{today}-{(todayCount + 1):D2}";
                sale.Timestamp = DateTime.Now;
                sale.Status = "Pending";

                product.Stock -= sale.Quantity;

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                return Ok(sale);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Sale create failed", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context.Sales.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to get sales", error = ex.Message });
            }
        }
    }
}