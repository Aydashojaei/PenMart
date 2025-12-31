using System.Collections.Generic;

namespace PenMart.Models
{
    public class Category
    {
        public Category()
        {
            Products = new List<Product>();
        }
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Discription { get; set; }
        public string Url { get; set; }

        public List<Product> Products { get; set; } //Navigation Property


    }
}
