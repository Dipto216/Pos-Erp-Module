using System.Net.Http;
using System.Net.Http.Json;
using POS.Web.Models;

namespace POS.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService(HttpClient http)
        {
            _http = http;
        }

    
        public async Task<List<Product>?> GetProducts()
        {
            
            try
            {
                return await _http.GetFromJsonAsync<List<Product>>("api/products");
            }
            catch
            {
                return new List<Product>();  
            }
        }

      
        public async Task CreateProduct(Product product)
        {
            
            try
            {
                await _http.PostAsJsonAsync("api/products", product);
            }
            catch
            {
                throw new Exception("API connection failed. Product not saved.");
            }
        }

       
        public async Task UpdateProduct(Product product)
        {
            await _http.PutAsJsonAsync($"api/products/{product.Id}", product);
        }

     
        public async Task DeleteProduct(int id)
        {
            await _http.DeleteAsync($"api/products/{id}");
        }

        public async Task<List<Sale>?> GetSales()
        {
            
            try
            {
                return await _http.GetFromJsonAsync<List<Sale>>("api/sales");
            }
            catch
            {
                return new List<Sale>();  
            }
        }

     
        public async Task CreateSale(Sale sale)
        {
            
            try
            {
                var response = await _http.PostAsJsonAsync("api/sales", sale);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
            catch
            {
                throw new Exception("API connection failed. Sale not saved.");
            }
        }


        public async Task Sync()
        {
           
            try
            {
                await _http.PostAsync("api/sync/sales", null);
            }
            catch
            {
                throw new Exception("Sync failed. Please try again.");
            }
        }
    }
}