using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PenMart.Models;
using System.Threading.Tasks;



namespace PenMart.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region Register
        public IActionResult Register()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistreViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            var existingUser = await _userManager.FindByEmailAsync(register.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "این ایمیل قبلا ثبت نام کرده است");
                return View(register);
            }

            ApplicationUser User = new ApplicationUser()
            {
                Email = register.Email.ToLower(),
                UserName = register.Email.ToLower(),
                RegisterDate = System.DateTime.Now,
                IsAdmin = false
            };
            var result = await _userManager.CreateAsync(User, register.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(User, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(register);
        }
        #endregion

        #region Login
     
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = await _userManager.FindByEmailAsync(login.Email.ToLower());
            if (user == null)
            {
                ModelState.AddModelError("Email", "اطلاعات صحیح نمیباشد");
                return View(login);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                login.Password,
                isPersistent: login.RemmemberMe,
               lockoutOnFailure: false
               );
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Email", "اطلاعات صحیح نمیباشد");
            return View(login);
        }
        #endregion
        #region Logout
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home/Index");
        }
        #endregion
    }
}
