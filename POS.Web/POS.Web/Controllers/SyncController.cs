using Microsoft.AspNetCore.Mvc;
using POS.Web.Services;

namespace POS.Web.Controllers
{
    public class SyncController : Controller
    {
        private readonly ApiService _api;

        public SyncController(ApiService api)
        {
            _api = api;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SyncNow()
        {
            
            try
            {
                await _api.Sync();
                TempData["Success"] = "Sales data synced successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Sync failed: {ex.Message}";  
            }

            return RedirectToAction("Index");
        }
    }
}