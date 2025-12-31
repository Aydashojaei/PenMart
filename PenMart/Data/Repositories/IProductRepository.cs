using PenMart.Models;
using System.Collections;
using System.Collections.Generic;

namespace PenMart.Data.Repositories
{
   
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<ProductViewModel> GetProductByCategory(int categoryId);
         Product GetProductById(int id);

       
    }
}
