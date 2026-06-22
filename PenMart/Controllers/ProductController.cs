using Microsoft.AspNetCore.Mvc;
using PenMart.Data.Repositories;
using PenMart.Models;
using System.Linq;

namespace PenMart.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

       public IActionResult ShowProductByCategoryId(int categoryId,string name)
        {
            ViewData["CategoryName"] = name;
            var products = _productRepository.GetProductByCategory(categoryId);
            return View(products);
        }

        public IActionResult ShowProductDetails(int id)
        {
            var product = _productRepository.GetProductById(id);
            if(product==null)
            {
                return NotFound();
            }
            var vm = new ProductDetailsViewModel()
            {
                Product = product,
                Item = product.Item,
                Images = product.Images

            };
            return View(vm);
        }

        public IActionResult ShowAllProduct()
        {
            var allproducts = _productRepository.GetAllProducts().Select(p => new ProductViewModel
            {
                Name = p.Name,
                Price = p.Item.Price,
                Url = p.MainImageUrl
            }).ToList();
            return View("ShowAllProduct", allproducts);
        }

        
    }
}
