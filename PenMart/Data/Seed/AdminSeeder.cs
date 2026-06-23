using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PenMart.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PenMart.Data.Seed
{
    /// <summary>
    /// Seeds the "Admin" role and a default admin user into the database on first run.
    /// Credentials can be changed after first login.
    /// </summary>
    public static class AdminSeeder
    {
        private const string AdminRole = "Admin";
        // Fallback values used only if "AdminSeed" is missing from configuration
        private const string DefaultAdminEmail = "admin@penmart.ir";
        private const string DefaultAdminPassword = "Admin@123456";

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
      
            using var scope = serviceProvider.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminEmail = configuration["AdminSeed:Email"] ?? DefaultAdminEmail;
            var adminPassword = configuration["AdminSeed:Password"] ?? DefaultAdminPassword;

            // Create "Admin" role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRole));
            }

            // Create default admin user if it doesn't exist
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    RegisterDate = DateTime.Now,
                    IsAdmin = true,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, AdminRole);
                }
            }
            else
            {
                // Make sure existing admin user has the Admin role
                if (!await userManager.IsInRoleAsync(adminUser, AdminRole))
                {
                    await userManager.AddToRoleAsync(adminUser, AdminRole);
                }
            }
        }
    }
}
