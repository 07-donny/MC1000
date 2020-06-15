using Microsoft.EntityFrameworkCore.Migrations;

namespace MC1000.Migrations
{
    public partial class product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Product_ProductId",
                table: "Discount");

            migrationBuilder.DropIndex(
                name: "IX_Discount_ProductId",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Discount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Discount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Discount_ProductId",
                table: "Discount",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Product_ProductId",
                table: "Discount",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
