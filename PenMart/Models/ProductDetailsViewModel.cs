using System.Collections.Generic;

namespace PenMart.Models
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public Item Item { get; set; }
        public List<ProductImage> Images { get; set; }
    }
}
