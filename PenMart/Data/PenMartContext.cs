using Microsoft.EntityFrameworkCore;
using PenMart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;






namespace PenMart.Data
{
    /// <summary>
    /// Application DbContext for PenMart. 
    /// Defines DbSets for main entities and configures relationships.
    /// </summary>
    public class PenMartContext : IdentityDbContext<ApplicationUser>
    {
        public PenMartContext(DbContextOptions<PenMartContext> options) : base(options)
        {

        }

        /// <summary>Products available in the store</summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>Specific sellable items (variant/stock/price)</summary>
        public DbSet<Item> Items { get; set; }

        /// <summary>Categories for grouping products</summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>Images associated with each product</summary>
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure decimal precision for price fields

            modelBuilder.Entity<Item>()
                .Property(i => i.Price)
                .HasColumnType("decimal(18,2)");

            // Configure one-to-many: Category -> Products

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            //// Configure one-to-many: Product -> ProductImages

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Product);

            #region SeedData Category
            modelBuilder.Entity<Category>()
                .HasData(new Category()
                {
                    Id = 1,
                    CategoryName = "نوشت افزار",
                    Discription = "انواع و اقسام خودکار ها و مداد ها در رنگ ها و مدل های مختلف",
                    Url= "/Images/Categoreis/pens5.PNG"


                },
                new Category()
                {
                    Id = 2,
                    CategoryName = "دفتر و کاغذ",
                    Discription = "انواع و اقسام دفتر و کاغذ در اندازه های مختلف",
                     Url = "/Images/Categoreis/papers.PNG"


                },
                new Category()
                {
                    Id = 3,
                    CategoryName = "لوازم نقاشی و هنری",
                    Discription = "انواع و اقسام رنگ، قلم‌مو، گواش و...",
                    Url = "/Images/Categoreis/naghashi.PNG"
                });
            #endregion

            #region SeedData Item
            modelBuilder.Entity<Item>()
                .HasData(new Item()
                {
                    Id = 1,
                    Price = 5000,
                    QuantityInStock = 10

                },
                new Item()
                {
                    Id = 2,
                    Price = 7000,
                    QuantityInStock = 20

                },
                new Item()
                {
                    Id = 3,
                    Price = 6500,
                    QuantityInStock = 45

                },
                new Item()
                {
                    Id = 4,
                    Price = 140000,
                    QuantityInStock = 35

                },
                new Item()
                {
                    Id = 5,
                    Price = 65000,
                    QuantityInStock = 30

                },
                new Item()
                {
                    Id = 6,
                    Price = 80000,
                    QuantityInStock = 17

                },
                new Item()
                {
                    Id = 7,
                    Price = 450000,
                    QuantityInStock = 20

                },
                 new Item()
                 {
                     Id = 8,
                     Price = 330000,
                     QuantityInStock = 22

                 },
                  new Item()
                  {
                      Id = 9,
                      Price = 664000,
                      QuantityInStock = 13

                  });
            #endregion

            #region SeedData Product
            modelBuilder.Entity<Product>()
                .HasData(new Product()
                {
                    Id = 1,
                    CategoryId = 1,
                    ItemId = 1,
                    Name = "خودکار بیک آبی",
                    Discription = "خودکار کلاسیک و پرکاربرد"

                },
                new Product()
                {
                    Id = 2,
                    CategoryId = 1,
                    ItemId = 2,
                    Name = "مداد ۲B فابر کاستل",
                    Discription = "مداد طراحی با کیفیت بالا"

                },
                new Product()
                {
                    Id = 3,
                    CategoryId = 1,
                    ItemId = 3,
                    Name = "مداد رنگی استدلر ۱۲ رنگ",
                    Discription = "مناسب برای طراحی و نقاشی"

                },
                new Product()
                {
                    Id = 4,
                    CategoryId = 2,
                    ItemId = 4,
                    Name = "دفتر ۱۰۰ برگ سیمی",
                    Discription = "دفتر سیمی مناسب برای یادداشت روزانه و مدرسه"

                },
                new Product()
                {
                    Id = 5,
                    CategoryId = 2,
                    ItemId = 5,
                    Name = "دفترچه یادداشت کوچک",
                    Discription = "دفترچه جیبی برای یادداشت سریع"

                },
                new Product()
                {
                    Id = 6,
                    CategoryId = 2,
                    ItemId = 6,
                    Name = "دفتر زبان ۷۰ برگ",
                    Discription = "دفتر مخصوص یادگیری و تمرین زبان"

                },
                new Product()
                {
                    Id = 7,
                    CategoryId = 3,
                    ItemId = 7,
                    Name = "گواش ۱۲ رنگ فابر کاستل",
                    Discription = "مناسب برای نقاشی و کارهای هنری کودک و بزرگسال"

                },
                new Product()
                {
                    Id = 8,
                    CategoryId = 3,
                    ItemId = 8,
                    Name = "آبرنگ ۱۲ رنگ استدلر",
                    Discription = "رنگ‌های با کیفیت و شفاف برای طراحی و نقاشی"

                },
                new Product()
                {
                    Id = 9,
                    CategoryId = 3,
                    ItemId = 9,
                    Name = "پالت رنگ پلاستیکی",
                    Discription = "برای ترکیب رنگ‌ها و نقاشی راحت‌تر"

                });
            #endregion


            #region SeedData ProductImage
            modelBuilder.Entity<ProductImage>()
                .HasData(
               new ProductImage() { Id = 1, Url = "/Images/Products/bic1.jpg", IsMain = true, ProductId = 1 },
               new ProductImage() { Id = 2, Url = "/Images/Products/bic2.jpg", IsMain = false, ProductId = 1 },
               new ProductImage() { Id = 3, Url = "/Images/Products/bic3.jpg", IsMain = false, ProductId = 1 },

                 new ProductImage() { Id = 4, Url = "/Images/Products/faber1.jpg", IsMain = true, ProductId = 2 },
                   new ProductImage() { Id = 5, Url = "/Images/Products/faber2.jpg", IsMain = false, ProductId = 2 },
                     new ProductImage() { Id = 6, Url = "/Images/Products/faber3.jpg", IsMain = false, ProductId = 2 },

                       new ProductImage() { Id = 7, Url = "/Images/Products/ravannevis1.png", IsMain = true, ProductId = 3 },
                         new ProductImage() { Id = 8, Url = "/Images/Products/ravannevis2.jpg", IsMain = false, ProductId = 3 },
                           new ProductImage() { Id = 9, Url = "/Images/Products/ravannevis3.jpg", IsMain = false, ProductId = 3 },

                            new ProductImage() { Id = 10, Url = "/Images/Products/daftar1.jpg", IsMain = true, ProductId = 4 },
                         new ProductImage() { Id = 11, Url = "/Images/Products/daftar2.jpg", IsMain = false, ProductId = 4 },
                           new ProductImage() { Id = 12, Url = "/Images/Products/daftar3.jpg", IsMain = false, ProductId = 4 },

                             new ProductImage() { Id = 13, Url = "/Images/Products/daftarche1.jpg", IsMain = true, ProductId = 5 },
                         new ProductImage() { Id = 14, Url = "/Images/Products/daftarche2.jpg", IsMain = false, ProductId = 5 },
                           new ProductImage() { Id = 15, Url = "/Images/Products/daftarche3.jpg", IsMain = false, ProductId = 5 },

                                  new ProductImage() { Id = 16, Url = "/Images/Products/daftarzaban1.jpg", IsMain = true, ProductId = 6 },
                         new ProductImage() { Id = 17, Url = "/Images/Products/daftarzaban2.jpg", IsMain = false, ProductId = 6 },
                           new ProductImage() { Id = 18, Url = "/Images/Products/daftarzaban3.jpg", IsMain = false, ProductId = 6 },

                           new ProductImage() { Id = 19, Url = "/Images/Products/guash1.jpg", IsMain = true, ProductId = 7 },
                         new ProductImage() { Id = 20, Url = "/Images/Products/guash2.jpg", IsMain = false, ProductId = 7 },
                           new ProductImage() { Id = 21, Url = "/Images/Products/guash3.jpg", IsMain = false, ProductId = 7 },

                              new ProductImage() { Id = 22, Url = "/Images/Products/abrang1.jpg", IsMain = true, ProductId = 8 },
                         new ProductImage() { Id = 23, Url = "/Images/Products/abrang2.jpg", IsMain = false, ProductId = 8 },
                           new ProductImage() { Id = 24, Url = "/Images/Products/abrang3.jpg", IsMain = false, ProductId = 8 },

                            new ProductImage() { Id = 25, Url = "/Images/Products/palet1.jpg", IsMain = true, ProductId = 9 },
                         new ProductImage() { Id = 26, Url = "/Images/Products/palet2.jpg", IsMain = false, ProductId = 9 },
                           new ProductImage() { Id = 27, Url = "/Images/Products/palet3.jpg", IsMain = false, ProductId = 9 }
               );
            #endregion


            base.OnModelCreating(modelBuilder);
        }


    }
}
