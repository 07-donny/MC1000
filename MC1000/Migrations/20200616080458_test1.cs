using Microsoft.EntityFrameworkCore.Migrations;

namespace MC1000.Migrations
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubSubCategoryId",
                table: "Product",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_SubSubCategoryId",
                table: "Product",
                column: "SubSubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_SubSubCategory_SubSubCategoryId",
                table: "Product",
                column: "SubSubCategoryId",
                principalTable: "SubSubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_SubSubCategory_SubSubCategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SubSubCategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SubSubCategoryId",
                table: "Product");
        }
    }
}
