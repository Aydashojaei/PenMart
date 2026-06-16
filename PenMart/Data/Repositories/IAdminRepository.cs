using PenMart.Models;
using System.Collections.Generic;

namespace PenMart.Data.Repositories
{
    public interface IAdminRepository
    {
        // ---- Product CRUD ----
        IEnumerable<Product> GetAllProductsWithDetails();
        Product GetProductByIdWithDetails(int id);
        void AddProduct(Product product, Item item);
        void UpdateProduct(Product product, Item item);
        void DeleteProduct(int id);

        // ---- Category CRUD ----
        IEnumerable<Category> GetAllCategoriesWithProducts();
        Category GetCategoryById(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);

        // ---- Order Management ----
        IEnumerable<Order> GetAllOrdersWithDetails();
        Order GetOrderById(int id);
        void FinalizeOrder(int orderId);
        void DeleteOrder(int orderId);
    }
}
