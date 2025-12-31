using Microsoft.EntityFrameworkCore.Migrations;

namespace PenMart.Migrations
{
    public partial class UpdateProductImageUrls3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("UPDATE ProductImages SET Url = REPLACE(Url, '/Images/Products/Product/', '/Images/Products/');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
