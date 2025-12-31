using Microsoft.EntityFrameworkCore.Migrations;

namespace PenMart.Migrations
{
    public partial class UpdateProductImageUrls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryName",
                value: "نوشت افزار");

            migrationBuilder.Sql("UPDATE ProductImages SET Url = REPLACE(Url, '/Images/', '/Images/Products/');");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryName",
                value: "خودکار و مداد");
        }
    }
}
