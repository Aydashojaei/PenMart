using PenMart.Models;
using System.Collections.Generic;

namespace PenMart.Data.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
    }
}
