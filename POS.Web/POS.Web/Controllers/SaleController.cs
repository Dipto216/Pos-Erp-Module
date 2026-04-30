using Microsoft.AspNetCore.Mvc;
using POS.Web.Models;
using POS.Web.Services;

namespace POS.Web.Controllers
{
    public class SaleController : Controller
    {
        private readonly ApiService _api;

        public SaleController(ApiService api)
        {
            _api = api;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _api.GetProducts() ?? new List<Product>();
            var sales = await _api.GetSales() ?? new List<Sale>();
            ViewBag.Products = products;
            return View(sales);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sale sale)
        {
            ModelState.Remove("Status");
            ModelState.Remove("SaleId");
            ModelState.Remove("Timestamp");

            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _api.GetProducts() ?? new List<Product>();
                var sales = await _api.GetSales() ?? new List<Sale>();
                return View("Index", sales);
            }

          

            try
            {
                await _api.CreateSale(sale);
                TempData["Success"] = "Sale created successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Sale failed: {ex.Message}";  
            }

            return RedirectToAction("Index");
        }
    }
}