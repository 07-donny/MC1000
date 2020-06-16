using Microsoft.EntityFrameworkCore.Migrations;

namespace MC1000.Migrations
{
    public partial class test4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubSubCategory",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "SubSub",
                table: "Product",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubSub",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "SubSubCategory",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
