using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PenMart.Models.Admin
{
    public class AdminProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DisplayName("نام محصول")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DisplayName("توضیحات")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DisplayName("قیمت (تومان)")]
        [Range(1, 999999999, ErrorMessage = "{0} باید بیشتر از صفر باشد")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DisplayName("موجودی انبار")]
        [Range(0, 99999, ErrorMessage = "موجودی نمی‌تواند منفی باشد")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "لطفا {0} را انتخاب کنید")]
        [DisplayName("دسته‌بندی")]
        public int CategoryId { get; set; }

        // Main image (required for create)
        [DisplayName("تصویر اصلی")]
        public IFormFile MainImageFile { get; set; }

        // Extra images (optional)
        [DisplayName("تصاویر اضافه (حداکثر 2 عکس)")]
        public List<IFormFile> ExtraImageFiles { get; set; } = new List<IFormFile>();

        // Existing images for edit view
        public string ExistingMainImageUrl { get; set; }
        public List<ProductImage> ExistingImages { get; set; } = new List<ProductImage>();

        // Available categories for dropdown
        public IEnumerable<Category> Categories { get; set; }
    }
}
