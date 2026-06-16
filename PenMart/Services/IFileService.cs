using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PenMart.Services
{
    public interface IFileService
    {
        Task<string> SaveProductImageAsync(IFormFile file);
        Task<string> SaveCategoryImageAsync(IFormFile file);
        void DeleteFile(string relativePath);
    }
}
