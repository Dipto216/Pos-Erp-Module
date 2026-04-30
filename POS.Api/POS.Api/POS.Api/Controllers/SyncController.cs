using Microsoft.AspNetCore.Mvc;
using POS.Api.Data;

namespace POS.Api.Controllers
{
    [ApiController]
    [Route("api/sync")]
    public class SyncController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SyncController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("sales")]
        public async Task<IActionResult> SyncSales()
        {
            try
            {
                var pending = _context.Sales
                    .Where(x => x.Status == "Pending")
                    .ToList();

                if (!pending.Any())
                    return Ok(new { message = "No pending sales to sync" });

                foreach (var sale in pending)
                {
                    sale.Status = "Synced";
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = $"{pending.Count} sale(s) synced successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Sync failed", error = ex.Message });
            }
        }
    }
}
