using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Api.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        public string? SaleId { get; set; }          

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? Barcode { get; set; }

        public DateTime Timestamp { get; set; }

        public string? Status { get; set; }        
    }
}