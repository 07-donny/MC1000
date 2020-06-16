using Microsoft.EntityFrameworkCore.Migrations;

namespace MC1000.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_SubSubCategory_SubSubCategoryId",
                table: "Product");

            migrationBuilder.AlterColumn<int>(
                name: "SubSubCategoryId",
                table: "Product",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SubSubCategory",
                table: "Product",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_SubSubCategory_SubSubCategoryId",
                table: "Product",
                column: "SubSubCategoryId",
                principalTable: "SubSubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_SubSubCategory_SubSubCategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SubSubCategory",
                table: "Product");

            migrationBuilder.AlterColumn<int>(
                name: "SubSubCategoryId",
                table: "Product",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_SubSubCategory_SubSubCategoryId",
                table: "Product",
                column: "SubSubCategoryId",
                principalTable: "SubSubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
