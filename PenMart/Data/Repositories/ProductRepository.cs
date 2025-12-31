using Microsoft.EntityFrameworkCore;
using PenMart.Models;
using System.Collections.Generic;
using System.Linq;

namespace PenMart.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private PenMartContext _context;
        public ProductRepository(PenMartContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.Include(c => c.Images)
                .Include(c=>c.Item);
                
        }

        public IEnumerable<ProductViewModel> GetProductByCategory(int categoryId)
        {
            return _context.Products.Where(c => c.CategoryId == categoryId)
                .Include(c => c.Images)
                .Select(c => new ProductViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Item.Price,
                    Url = c.Images.FirstOrDefault(i => i.IsMain).Url


                }).ToList();

                
        }

        public Product GetProductById(int id)
        {
            return _context.Products
                .Include(p=> p.Item)
                .Include(p=> p.Images)
                .Include(p=> p.Category)
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
