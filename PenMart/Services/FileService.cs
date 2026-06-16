using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PenMart.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void DeleteFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            var fullPath = Path.Combine(
                _env.WebRootPath,
                relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<string> SaveProductImageAsync(IFormFile file)
        {
            return await SaveFileAsync(file, Path.Combine("Images", "Products"));
        }

        public async Task<string> SaveCategoryImageAsync(IFormFile file)
        {
            return await SaveFileAsync(file, Path.Combine("Images", "Categoreis"));
        }

        private async Task<string> SaveFileAsync(IFormFile file, string subFolder)
        {
            var uploadDir = Path.Combine(_env.WebRootPath, subFolder);
            Directory.CreateDirectory(uploadDir);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var absPath = Path.Combine(uploadDir, fileName);

            await using (var stream = File.Create(absPath))
            {
                await file.CopyToAsync(stream);
            }

            // Return URL with forward slashes and leading /
            return "/" + subFolder.Replace(Path.DirectorySeparatorChar, '/') + "/" + fileName;
        }
    }
}
