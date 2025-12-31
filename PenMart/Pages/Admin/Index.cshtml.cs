using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PenMart.Data.Repositories;
using PenMart.Models;
using System.Collections.Generic;
using System.Linq;

namespace PenMart.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(IProductRepository productRepository,ICategoryRepository categoryRepository,UserManager<ApplicationUser> userManager)
        {
            _productRepository= productRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        public int productCount { get; set; }
        public int categorycount { get; set; }
        public int userCount{ get; set; }
        public int orderCount { get; set; }
        public List<Order> order { get; set; }


        public void OnGet()
        {
            productCount = _productRepository.GetAllProducts().Count(); 
           categorycount=_categoryRepository.GetAllCategories().Count();
           userCount=_userManager.Users.Count();
           orderCount=order?.Count() ?? 0;
            
        }
        public void OnPost()
        {
        }
    }
}
