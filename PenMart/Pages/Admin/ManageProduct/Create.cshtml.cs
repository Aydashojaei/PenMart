using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PenMart.Data;
using PenMart.Data.Repositories;
using PenMart.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace PenMart.Pages.Admin.ManageProduct
{
    public class CreateModel : PageModel
    {
        // _context یعنی دسترسی به دیتابیس با EF Core
        private readonly PenMartContext _context;
        // _env یعنی دسترسی به محیط اجرا (مهم‌ترینش مسیر wwwroot)
        private readonly IWebHostEnvironment _env;
        public CreateModel(PenMartContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // قانون: حداکثر چند عکس اجازه داریم
        private const int MaxImages = 4;
        // قانون: حداکثر حجم هر فایل (اینجا ۲ مگ گذاشتیم)

        private const long MaxFileBytes = 2 * 1024 * 1024;

        // لیست نوع فایل‌های مجاز؛ یعنی فقط این فرمت‌ها رو قبول کن
        // HashSet سریع‌تر از List هست برای Contains
        private static readonly HashSet<string> AllowedContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };

        // این کلاس DTO هست: یعنی چیزهایی که از فرم می‌گیریم
        public class CreatProductInput
        {
            [DisplayName("نام محصول")]
            [Required(ErrorMessage = "نام محصول الزامی است")]
            public string Name { get; set; }

            [DisplayName("توضیحات")]
            public string Discription { get; set; }

            [DisplayName("قیمت")]
            [Required(ErrorMessage = "قیمت محصول را وارد کنید")]
            [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "قیمت باید بزرگتر از صفر باشد")]
            public decimal Price { get; set; }

            [DisplayName("موجودی")]
            [Required(ErrorMessage = "تعداد موجودی محصول را وارد کنید")]
            [Range(1, int.MaxValue, ErrorMessage = "موجودی باید حداقل 1 باشد")]
            public int QuantityStock { get; set; }

            [DisplayName("دسته بندی")]
            [Required(ErrorMessage = "دسته بندی محصول را مشخص کنید")]
            public int CategoryId { get; set; }

            [DisplayName("عکس اصلی")]
            public int MainImageIndex { get; set; } = 1;

            [DisplayName("تصاویر محصول")]
            public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        }


        // این Input همون چیزیه که فرم ما پر می‌کنه.
        // یعنی کاربر هرچی تو فرم بزنه میاد می‌شینه داخل Input.
        [BindProperty]
        public CreatProductInput Input { get; set; } = new CreatProductInput();

        // وقتی کاربر فقط صفحه رو باز می‌کنه (Submit نکرده)، این اجرا میشه.
        public IActionResult OnGet()
        {
            // Dropdown دسته‌بندی‌ها رو پر می‌کنیم که تو View نشون داده بشه.
            FillCategories();
            // پیش‌فرض می‌گیم عکس اول، عکس اصلیه (اگر کاربر چیزی تغییر نده)
            Input.MainImageIndex = 1;
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            FillCategories();

            //  اول می‌گیم: آیا فیلدهای معمولی فرم درست پر شده؟
            // مثلا Name خالی نباشه، Category انتخاب شده باشه و...
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //  حالا می‌ریم سراغ عکس‌ها: حداقل یک عکس باید باشد
            if (Input.Images is null|| Input.Images.Count==0)
            {
                ModelState.AddModelError(nameof(Input.Images), "حداقل یک عکس باید انتخاب شود");
            }

            // حداکثر ۴ عکس
            if (Input.Images!=null && Input.Images.Count>4)
            {
                ModelState.AddModelError(nameof(Input.Images), "حداکثر 4 عکس میتوانید انتخاب کنید");
            }
            // عکس اصلی فقط می‌تونه بین عکس‌هایی باشه که واقعا انتخاب شدند
            // یعنی اگر ۲ عکس انتخاب شده، Main فقط ۱ یا ۲ می‌تونه باشه
            if (Input.Images!=null && Input.Images.Count>0)
            {
                if(Input.MainImageIndex<1 || Input.MainImageIndex>Input.Images.Count)
                {
                    ModelState.AddModelError(nameof(Input.MainImageIndex), "عکس اصلی باید از عکس های انتخاب شده باشد ");
                }
            }
            //حجمش زیاد نباشه، فرمتش مجاز باشه، خالی نباشه
            if (Input.Images!=null)
            {
                for(int i=0;i<Input.Images.Count; i++)
                {
                    var file = Input.Images[i];
                    if(file==null ||file.Length==0)
                    {
                        ModelState.AddModelError(nameof(Input.Images), $"فایل شماره ی {i + 1}خالی است");
                        continue;
                    }

                    if(file.Length>MaxFileBytes)
                    {
                        ModelState.AddModelError(nameof(Input.Images), $"حجم فایل شماره{i + 1}بیش از 2 مگ است");
                    } 
                    if(!AllowedContentTypes.Contains(file.ContentType))
                    {
                        ModelState.AddModelError(nameof(Input.Images), $"فرمت فایل شماره ی {i + 1}مجاز نیست");
                    }
                }
            }
            // اگر هرکدوم از چک‌های بالا خطا اضافه کرده باشند، اینجا می‌فهمیم

            if (!ModelState.IsValid)
            {
                return Page();
            }
            //ساختن مسیر پوشه‌ی ذخیره عکس‌ها روی دیسک
            var UploadDir = Path.Combine(_env.WebRootPath, "Images", "Products");
            // اگر پوشه نبود، بساز؛ اگر بود کاری نکن
            Directory.CreateDirectory(UploadDir);

            //یه لییست میسازیک که توش ادرس  فایل هایی هست که روی سیستممون ذخیره شده که اگر وسط کار خطا گرفتیم فایل ها توش نمونه الکی بره پاک کنه 
            var SavedfilePath = new List<string>();
            //این هم برای اینه که باز اگر به مشکل خوردیم وسط کار درواقع دیتابیس رکور ازش پاک شه و برگرده به حالت قبل و اینکه یا همه چیز باهمذخیره میشه داخلش یا اگه یکیش نشد کلا نمیشه و برمیگره به عقب 
            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var item = new Item
                {
                    Price = Input.Price,
                    QuantityInStock = Input.QuantityStock
                };
                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                var product = new Product
                {
                    Name = Input.Name,
                    Discription = Input.Discription,
                    CategoryId = Input.CategoryId,

                    ItemId = item.Id,
                    Item = item,
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var productImages = new List<ProductImage>();

                for(int i=0;i<Input.Images.Count;i++)
                {
                    var file = Input.Images[i];
                    // پسوند فایل رو می‌گیریم (مثلاً .jpg)
                    var ext = Path.GetExtension(file.FileName);
                    // یک اسم یکتا می‌سازیم که با هیچ فایل دیگه‌ای قاطی نشه
                    var fileName = $"{Guid.NewGuid():N}{ext}";

                    // مسیر کامل فیزیکی فایل روی هارد

                    var absPath = Path.Combine(UploadDir, fileName);

                    // فایل رو روی دیسک می‌نویسیم
                    await using (var stream= System.IO.File.Create(absPath))
                    {
                        await file.CopyToAsync(stream);
                    }
                    // مسیر رو ذخیره می‌کنیم که اگر خطا شد پاکش کنیم
                    SavedfilePath.Add(absPath);

                    productImages.Add(new ProductImage
                    {

                        // این تصویر مربوط به کدوم محصوله؟
                        ProductId = product.Id,
                        // آدرس قابل استفاده در سایت (نه مسیر فیزیکی)
                        Url = "Images/Products/" + fileName,
                        // اگر این فایل، همون شماره‌ای بود که کاربر به عنوان عکس اصلی انتخاب کرده
                        IsMain = (i + 1) == Input.MainImageIndex
                    });
                
                }
                _context.ProductImages.AddRange(productImages);
                await _context.SaveChangesAsync();


                // خیلی مهم: اینجا یعنی "همه چیز موفق بود، قطعی کن"
                await tx.CommitAsync();

                // دقیقاً اینجاست که باید Redirect کنی (مسیر موفقیت)
                return RedirectToPage("Index");

            }




            catch
            {
                //هر خطایی در هنگام ذخیره سازی در دیتا بس رخ دهد برمیگیردد به حالت اول 
                await tx.RollbackAsync();
                //و اینجا هم  فایل ها یی که تو پوشه موندنو حذف میکنیم 
                foreach(var path in SavedfilePath)
                {
                    try
                    {
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);

                    }
                    catch
                    {

                    }
                    
                }
                ModelState.AddModelError(string.Empty, "خطایی هنگام ذخیره محصول رخ داد. لطفاً دوباره تلاش کنید.");
                return Page(); // ✅ بیرون foreach
            }

           

        }


        // این فقط برای پر کردن Dropdown دسته‌بندی‌هاست
        private void FillCategories()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
        }


    }
}
