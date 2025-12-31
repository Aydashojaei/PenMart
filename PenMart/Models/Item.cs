using System.ComponentModel;

namespace PenMart.Models
{
    public class Item
    {
        public int Id { get; set; }
        [DisplayName("قیمت")]
        public decimal Price { get; set; }
        [DisplayName("موجودی")]
        public int QuantityInStock { get; set; }

        //Navigation Property
        public Product Product { get; set; }                     
    }
}
