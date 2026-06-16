using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PenMart.Data;
using PenMart.Data.Repositories;
using PenMart.Models;
using PenMart.Services;
using PenMart.Data.Seed;

namespace PenMart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            #region Db Context
            // Register PenMart DbContext with SQL Server connection

            services.AddDbContext<PenMartContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });
            #endregion

            #region IOC Ctegoreis
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            #endregion

            #region IOC Products
            services.AddScoped<IProductRepository, ProductRepository>();
            #endregion

            #region IOC FileService

            services.AddScoped<IFileService, FileService>();

            #endregion

            #region IOC Admin

            services.AddScoped<IAdminRepository, AdminRepository>();

            #endregion

            #region AddIdentity

            services.AddIdentity<ApplicationUser, IdentityRole>() //این سه خط کل سیستم مدیریت کاربر (ثبت‌نام، ورود، نقش‌ها، توکن‌ها و امنیت) رو در پروژه‌ی تو فعال می‌کنه.
                .AddEntityFrameworkStores<PenMartContext>()
                .AddDefaultTokenProviders();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            // Seed admin role and default admin user on startup
            AdminSeeder.SeedAsync(app.ApplicationServices).GetAwaiter().GetResult();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
