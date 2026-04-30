using Microsoft.AspNetCore.Mvc;
using POS.Web.Models;
using POS.Web.Services;

namespace POS.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApiService _api;

        public ProductController(ApiService api)
        {
            _api = api;
        }

   
        public async Task<IActionResult> Index()
        {
            var products = await _api.GetProducts();

            if (products == null)
                products = new List<Product>();

            return View(products);
        }

     
        public IActionResult Create()
        {
            return View();
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
 
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            await _api.CreateProduct(product);

         
            TempData["Success"] = $"Product '{product.Name}' successfully added!";

            return RedirectToAction(nameof(Create));  
        }

      

      
  
        
    }
}