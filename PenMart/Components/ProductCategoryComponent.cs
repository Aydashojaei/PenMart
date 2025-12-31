using Microsoft.AspNetCore.Mvc;
using PenMart.Data.Repositories;
using System.Threading.Tasks;

namespace PenMart.Components
{
    public class ProductCategoryComponent:ViewComponent
    {
        private ICategoryRepository _categoryRepository;
        public ProductCategoryComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string view = "Default")
        {
            var Categories = _categoryRepository.GetAllCategories();
            return View($"/Views/Components/{view}.cshtml", Categories);

        }
    }
}
