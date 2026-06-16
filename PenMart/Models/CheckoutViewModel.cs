using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PenMart.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DisplayName("نام و نام خانوادگی")]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DisplayName("شماره موبایل")]
        [Phone(ErrorMessage = "شماره موبایل معتبر نیست")]
        [MaxLength(11)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DisplayName("آدرس تحویل")]
        [MaxLength(500)]
        public string Address { get; set; }

        [DisplayName("توضیحات (اختیاری)")]
        [MaxLength(300)]
        public string Notes { get; set; }

        // For display only
        public Order Order { get; set; }
    }
}
