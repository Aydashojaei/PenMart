using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PenMart.Models;
using System;
using System.Threading.Tasks;

namespace PenMart.Data.Seed
{
    /// <summary>
    /// Seeds the "Admin" role and a default admin user into the database on first run.
    /// Credentials can be changed after first login.
    /// </summary>
    public static class AdminSeeder
    {
        private const string AdminRole = "Admin";
        private const string AdminEmail = "admin@penmart.ir";
        private const string AdminPassword = "Admin@123456";

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create "Admin" role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRole));
            }

            // Create default admin user if it doesn't exist
            var adminUser = await userManager.FindByEmailAsync(AdminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    RegisterDate = DateTime.Now,
                    IsAdmin = true,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, AdminPassword);
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
