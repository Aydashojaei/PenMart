using Microsoft.EntityFrameworkCore.Migrations;

namespace PenMart.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Discription",
                table: "Categories",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "Discription" },
                values: new object[,]
                {
                    { 1, "خودکار و مداد", "انواع و اقسام خودکار ها و مداد ها در رنگ ها و مدل های مختلف" },
                    { 2, "دفتر و کاغذ", "انواع و اقسام دفتر و کاغذ در اندازه های مختلف" },
                    { 3, "لوازم نقاشی و هنری", "انواع و اقسام رنگ، قلم‌مو، گواش و..." }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Price", "QuantityInStock" },
                values: new object[,]
                {
                    { 1, 5000m, 10 },
                    { 2, 7000m, 20 },
                    { 3, 6500m, 45 },
                    { 4, 140000m, 35 },
                    { 5, 65000m, 30 },
                    { 6, 80000m, 17 },
                    { 7, 450000m, 20 },
                    { 8, 330000m, 22 },
                    { 9, 664000m, 13 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Discription", "ItemId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "خودکار کلاسیک و پرکاربرد", 1, "خودکار بیک آبی" },
                    { 2, 1, "مداد طراحی با کیفیت بالا", 2, "مداد ۲B فابر کاستل" },
                    { 3, 1, "مناسب برای طراحی و نقاشی", 3, "مداد رنگی استدلر ۱۲ رنگ" },
                    { 4, 2, "دفتر سیمی مناسب برای یادداشت روزانه و مدرسه", 4, "دفتر ۱۰۰ برگ سیمی" },
                    { 5, 2, "دفترچه جیبی برای یادداشت سریع", 5, "دفترچه یادداشت کوچک" },
                    { 6, 2, "دفتر مخصوص یادگیری و تمرین زبان", 6, "دفتر زبان ۷۰ برگ" },
                    { 7, 3, "مناسب برای نقاشی و کارهای هنری کودک و بزرگسال", 7, "گواش ۱۲ رنگ فابر کاستل" },
                    { 8, 3, "رنگ‌های با کیفیت و شفاف برای طراحی و نقاشی", 8, "آبرنگ ۱۲ رنگ استدلر" },
                    { 9, 3, "برای ترکیب رنگ‌ها و نقاشی راحت‌تر", 9, "پالت رنگ پلاستیکی" }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "IsMain", "ProductId", "Url" },
                values: new object[,]
                {
                    { 1, true, 1, "/Images/bic1.jpg" },
                    { 25, true, 9, "/Images/palet1.jpg" },
                    { 24, false, 8, "/Images/abrang3.jpg" },
                    { 23, false, 8, "/Images/abrang2.jpg" },
                    { 22, true, 8, "/Images/abrang1.jpg" },
                    { 21, false, 7, "/Images/guash3.jpg" },
                    { 20, false, 7, "/Images/guash2.jpg" },
                    { 19, true, 7, "/Images/guash1.jpg" },
                    { 18, false, 6, "/Images/daftarzaban3.jpg" },
                    { 17, false, 6, "/Images/daftarzaban2.jpg" },
                    { 16, true, 6, "/Images/daftarzaban1.jpg" },
                    { 15, false, 5, "/Images/daftarche3.jpg" },
                    { 26, false, 9, "/Images/palet2.jpg" },
                    { 14, false, 5, "/Images/daftarche2.jpg" },
                    { 12, false, 4, "/Images/daftar3.jpg" },
                    { 11, false, 4, "/Images/daftar2.jpg" },
                    { 10, true, 4, "/Images/daftar1.jpg" },
                    { 9, false, 3, "/Images/ravannevis3.jpg" },
                    { 8, false, 3, "/Images/ravannevis2.jpg" },
                    { 7, true, 3, "/Images/ravannevis1.png" },
                    { 6, false, 2, "/Images/faber3.jpg" },
                    { 5, false, 2, "/Images/faber2.jpg" },
                    { 4, true, 2, "/Images/faber1.jpg" },
                    { 3, false, 1, "/Images/bic3.jpg" },
                    { 2, false, 1, "/Images/bic2.jpg" },
                    { 13, true, 5, "/Images/daftarche1.jpg" },
                    { 27, false, 9, "/Images/palet3.jpg" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "Discription",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
