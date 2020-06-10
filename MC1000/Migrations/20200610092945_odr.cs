using Microsoft.EntityFrameworkCore.Migrations;

namespace MC1000.Migrations
{
    public partial class odr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLine_Promotion_PromotionId",
                table: "OrderLine");

            migrationBuilder.DropIndex(
                name: "IX_OrderLine_PromotionId",
                table: "OrderLine");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "OrderLine");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "OrderLine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_PromotionId",
                table: "OrderLine",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLine_Promotion_PromotionId",
                table: "OrderLine",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
