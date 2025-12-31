using Microsoft.EntityFrameworkCore;
using PenMart.Models;
using System.Collections.Generic;

namespace PenMart.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
       private PenMartContext _context;
        public CategoryRepository(PenMartContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories;
                
        }
    }
}
