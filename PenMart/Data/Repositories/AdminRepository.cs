using Microsoft.EntityFrameworkCore;
using PenMart.Models;
using System.Collections.Generic;
using System.Linq;

namespace PenMart.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly PenMartContext _context;

        public AdminRepository(PenMartContext context)
        {
            _context = context;
        }

        // ---- Product CRUD ----

        public IEnumerable<Product> GetAllProductsWithDetails()
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Item)
                .Include(p => p.Images)
                .OrderBy(p => p.CategoryId)
                .ThenBy(p => p.Name)
                .ToList();
        }

        public Product GetProductByIdWithDetails(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Item)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product, Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();

            product.ItemId = item.Id;
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product, Item item)
        {
            _context.Items.Update(item);
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products
                .Include(p => p.Item)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return;

            // Remove associated images and item records first
            _context.ProductImages.RemoveRange(product.Images);
            _context.Products.Remove(product);

            if (product.Item != null)
                _context.Items.Remove(product.Item);

            _context.SaveChanges();
        }

        // ---- Category CRUD ----

        public IEnumerable<Category> GetAllCategoriesWithProducts()
        {
            return _context.Categories
                .Include(c => c.Products)
                .OrderBy(c => c.CategoryName)
                .ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);

            if (category == null) return;

            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        // ---- Order Management ----

        public IEnumerable<Order> GetAllOrdersWithDetails()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.orderDetails)
                    .ThenInclude(d => d.product)
                .OrderByDescending(o => o.CreatDate)
                .ToList();
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.orderDetails)
                    .ThenInclude(d => d.product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefault(o => o.OrderId == id);
        }

        public void FinalizeOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null) return;

            order.IsFinaly = true;
            _context.SaveChanges();
        }

        public void DeleteOrder(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.orderDetails)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null) return;

            _context.OrderDetails.RemoveRange(order.orderDetails);
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }
}
