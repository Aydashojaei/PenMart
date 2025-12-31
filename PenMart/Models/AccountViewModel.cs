using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PenMart.Models
{
   public class RegistreViewModel
    {
        [Required(ErrorMessage ="لطفا {0}را وارد کنید"),MaxLength(300)]
        [EmailAddress(ErrorMessage = "لطفا فرمت {0} را درست وارد کنید")]
        [DisplayName("ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0}را وارد کنید"), MaxLength(50)]
        [DataType(DataType.Password)]
        [DisplayName("کلمه عبور")]

        public string Password{ get; set; }

        [Required(ErrorMessage = "لطفا {0}را وارد کنید"), MaxLength(50)]
        [DataType(DataType.Password)]
        [DisplayName("تکرار کلمه عبور")]
        [Compare("Password",ErrorMessage ="رمز عبور و تکرار آن یکسان نیست")]

        public string RePassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage ="لطفا {0} خود را وارد کنید"),MaxLength(300)]
        [EmailAddress(ErrorMessage ="لطفا فزمت {0} را درست وارد کنید")]
        [DisplayName("ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0}را وارد کنید"), MaxLength(50)]
        [DataType(DataType.Password)]
        [DisplayName("کلمه عبور")]
        public string Password { get; set; }
        [DisplayName("مرا به خاطر بسپار")]
        public bool RemmemberMe { get; set; }

    }
}
