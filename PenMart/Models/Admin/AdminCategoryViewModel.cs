using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PenMart.Models.Admin
{
    public class AdminCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DisplayName("نام دسته‌بندی")]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DisplayName("توضیحات")]
        [MaxLength(500)]
        public string Description { get; set; }

        [DisplayName("تصویر دسته‌بندی")]
        public IFormFile ImageFile { get; set; }

        public string ExistingImageUrl { get; set; }
    }
}
