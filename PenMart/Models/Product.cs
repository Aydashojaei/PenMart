using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PenMart.Models
{
    public class Product
    {
        public Product()
        {
          Images = new List<ProductImage>();
        }
        public int Id { get; set; }
        [DisplayName("نام محصول")]
        public string Name { get; set; }
        [DisplayName("توضیحات")]
        public string  Discription { get; set; }

       
        public Item Item { get; set; }  //Navigation Property
        public Category Category { get; set; }//Navigation Property
        public List<ProductImage> Images { get; set; }//Navigation Property
        public string MainImageUrl => Images?.FirstOrDefault(i => i.IsMain)?.Url;
        public int ItemId { get; set; } //Foreign key
        [DisplayName("گروه محصول")]
        public int CategoryId { get; set; } //Foreign key
        
    }
}
