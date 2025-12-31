namespace PenMart.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public int ProductId { get; set; } //Foreign key
        public Product Product { get; set; }//Navigation property

    }
}
