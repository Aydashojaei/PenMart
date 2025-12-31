using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PenMart.Data;
using PenMart.Data.Repositories;
using PenMart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenMart.Pages.Admin.ManageProduct
{
    public class IndexModel : PageModel
    {
        private readonly IProductRepository _productReposiory;
        public IndexModel(IProductRepository productRepository)
        {
            _productReposiory = productRepository;
        }
        public IEnumerable<Product> products { get; set; }
        public void OnGet()
        {
            products = _productReposiory.GetAllProducts();

        }
        public void OnPost()
        {
        }
    }
}
