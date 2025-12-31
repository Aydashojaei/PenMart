using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PenMart.Models
{
    public class OrderDetail
    {
        [Key]
        public int DetailId { get; set; }
        [Required]
        public int OrderId { get; set; } //ForeignKey
        [Required]
        public int ProductId { get; set; } //ForeignKey
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Count { get; set; }

        public Order order { get; set; } //Navigation Property
        [ForeignKey("ProductId")]
        public Product product { get; set; } //Navigation Property
    }
}
