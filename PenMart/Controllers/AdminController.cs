using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PenMart.Data.Repositories;
using PenMart.Models;
using PenMart.Models.Admin;
using PenMart.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PenMart.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileService _fileService;

        public AdminController(
            IAdminRepository adminRepository,
            ICategoryRepository categoryRepository,
            IFileService fileService)
        {
            _adminRepository = adminRepository;
            _categoryRepository = categoryRepository;
            _fileService = fileService;
        }

        // ===================== DASHBOARD =====================

        public IActionResult Index()
        {
            var products = _adminRepository.GetAllProductsWithDetails();
            var categories = _adminRepository.GetAllCategoriesWithProducts();
            var orders = _adminRepository.GetAllOrdersWithDetails();

            ViewBag.TotalProducts = products.Count();
            ViewBag.TotalCategories = categories.Count();
            ViewBag.TotalOrders = orders.Count();
            ViewBag.PendingOrders = orders.Count(o => !o.IsFinaly);

            return View();
        }

        // ===================== PRODUCTS =====================

        public IActionResult Products()
        {
            var products = _adminRepository.GetAllProductsWithDetails();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            var vm = new AdminProductViewModel
            {
                Categories = _categoryRepository.GetAllCategories()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(AdminProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = _categoryRepository.GetAllCategories();
                return View(vm);
            }

            var item = new Item
            {
                Price = vm.Price,
                QuantityInStock = vm.QuantityInStock
            };

            var product = new Product
            {
                Name = vm.Name,
                Discription = vm.Description,
                CategoryId = vm.CategoryId
            };

            // Main image (required)
            if (vm.MainImageFile != null && vm.MainImageFile.Length > 0)
            {
                var mainUrl = await _fileService.SaveProductImageAsync(vm.MainImageFile);
                product.Images.Add(new ProductImage { Url = mainUrl, IsMain = true });
            }
            else
            {
                product.Images.Add(new ProductImage { Url = "/Images/no-image.png", IsMain = true });
            }

            // Extra images (optional, max 2)
            if (vm.ExtraImageFiles != null)
            {
                int extraCount = 0;
                foreach (var extraFile in vm.ExtraImageFiles)
                {
                    if (extraFile != null && extraFile.Length > 0 && extraCount < 2)
                    {
                        var extraUrl = await _fileService.SaveProductImageAsync(extraFile);
                        product.Images.Add(new ProductImage { Url = extraUrl, IsMain = false });
                        extraCount++;
                    }
                }
            }

            _adminRepository.AddProduct(product, item);

            TempData["Success"] = $"محصول «{product.Name}» با موفقیت اضافه شد.";
            return RedirectToAction(nameof(Products));
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _adminRepository.GetProductByIdWithDetails(id);
            if (product == null) return NotFound();

            var vm = new AdminProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Discription,
                Price = product.Item?.Price ?? 0,
                QuantityInStock = product.Item?.QuantityInStock ?? 0,
                CategoryId = product.CategoryId,
                ExistingMainImageUrl = product.MainImageUrl,
                ExistingImages = product.Images ?? new System.Collections.Generic.List<ProductImage>(),
                Categories = _categoryRepository.GetAllCategories()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(AdminProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = _categoryRepository.GetAllCategories();
                return View(vm);
            }

            var product = _adminRepository.GetProductByIdWithDetails(vm.Id);
            if (product == null) return NotFound();

            // Update main image if new one uploaded
            if (vm.MainImageFile != null && vm.MainImageFile.Length > 0)
            {
                var oldMain = product.Images?.FirstOrDefault(i => i.IsMain);
                if (oldMain != null)
                    _fileService.DeleteFile(oldMain.Url);

                var newUrl = await _fileService.SaveProductImageAsync(vm.MainImageFile);

                if (oldMain != null)
                    oldMain.Url = newUrl;
                else
                    product.Images.Add(new ProductImage { Url = newUrl, IsMain = true, ProductId = product.Id });
            }

            // Add extra images if uploaded
            if (vm.ExtraImageFiles != null)
            {
                int currentExtras = product.Images?.Count(i => !i.IsMain) ?? 0;
                foreach (var extraFile in vm.ExtraImageFiles)
                {
                    if (extraFile != null && extraFile.Length > 0 && currentExtras < 2)
                    {
                        var extraUrl = await _fileService.SaveProductImageAsync(extraFile);
                        product.Images.Add(new ProductImage { Url = extraUrl, IsMain = false, ProductId = product.Id });
                        currentExtras++;
                    }
                }
            }

            product.Name = vm.Name;
            product.Discription = vm.Description;
            product.CategoryId = vm.CategoryId;
            product.Item.Price = vm.Price;
            product.Item.QuantityInStock = vm.QuantityInStock;

            _adminRepository.UpdateProduct(product, product.Item);

            TempData["Success"] = $"محصول «{product.Name}» با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Products));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int id)
        {
            var product = _adminRepository.GetProductByIdWithDetails(id);
            if (product == null) return NotFound();

            // Delete all image files from disk
            if (product.Images != null)
            {
                foreach (var img in product.Images)
                    _fileService.DeleteFile(img.Url);
            }

            _adminRepository.DeleteProduct(id);

            TempData["Success"] = "محصول با موفقیت حذف شد.";
            return RedirectToAction(nameof(Products));
        }

        // ===================== CATEGORIES =====================

        public IActionResult Categories()
        {
            var categories = _adminRepository.GetAllCategoriesWithProducts();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View(new AdminCategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(AdminCategoryViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            string imageUrl = "/Images/Categoreis/default.png";

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
                imageUrl = await _fileService.SaveCategoryImageAsync(vm.ImageFile);

            var category = new Category
            {
                CategoryName = vm.CategoryName,
                Discription = vm.Description,
                Url = imageUrl
            };

            _adminRepository.AddCategory(category);

            TempData["Success"] = $"دسته‌بندی «{category.CategoryName}» با موفقیت اضافه شد.";
            return RedirectToAction(nameof(Categories));
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category = _adminRepository.GetCategoryById(id);
            if (category == null) return NotFound();

            var vm = new AdminCategoryViewModel
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Discription,
                ExistingImageUrl = category.Url
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(AdminCategoryViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var category = _adminRepository.GetCategoryById(vm.Id);
            if (category == null) return NotFound();

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                _fileService.DeleteFile(category.Url);
                category.Url = await _fileService.SaveCategoryImageAsync(vm.ImageFile);
            }

            category.CategoryName = vm.CategoryName;
            category.Discription = vm.Description;

            _adminRepository.UpdateCategory(category);

            TempData["Success"] = $"دسته‌بندی «{category.CategoryName}» با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            var category = _adminRepository.GetCategoryById(id);
            if (category == null) return NotFound();

            if (category.Products != null && category.Products.Count > 0)
            {
                TempData["Error"] = "این دسته‌بندی دارای محصول است. ابتدا محصولات آن را حذف کنید.";
                return RedirectToAction(nameof(Categories));
            }

            _adminRepository.DeleteCategory(id);

            TempData["Success"] = "دسته‌بندی با موفقیت حذف شد.";
            return RedirectToAction(nameof(Categories));
        }

        // ===================== ORDERS =====================

        public IActionResult Orders()
        {
            var orders = _adminRepository.GetAllOrdersWithDetails();

            var vm = orders.Select(o => new AdminOrderViewModel
            {
                OrderId = o.OrderId,
                UserEmail = o.User?.Email ?? "نامشخص",
                CreatedDate = o.CreatDate,
                IsFinalized = o.IsFinaly,
                TotalAmount = o.orderDetails?.Sum(d => d.Count * d.Price) ?? 0,
                ItemCount = o.orderDetails?.Sum(d => d.Count) ?? 0
            });

            return View(vm);
        }

        public IActionResult OrderDetails(int id)
        {
            var order = _adminRepository.GetOrderById(id);
            if (order == null) return NotFound();

            var vm = new AdminOrderDetailViewModel
            {
                OrderId = order.OrderId,
                UserEmail = order.User?.Email ?? "نامشخص",
                CreatedDate = order.CreatDate,
                IsFinalized = order.IsFinaly,
                OrderDetails = order.orderDetails,
                TotalAmount = order.orderDetails?.Sum(d => d.Count * d.Price) ?? 0
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizeOrder(int id)
        {
            _adminRepository.FinalizeOrder(id);
            TempData["Success"] = "سفارش با موفقیت نهایی شد.";
            return RedirectToAction(nameof(Orders));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOrder(int id)
        {
            _adminRepository.DeleteOrder(id);
            TempData["Success"] = "سفارش با موفقیت حذف شد.";
            return RedirectToAction(nameof(Orders));
        }
    }
}
