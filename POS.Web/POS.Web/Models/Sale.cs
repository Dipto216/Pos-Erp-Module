namespace POS.Web.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string? SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Barcode { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Status { get; set; }
    }
}