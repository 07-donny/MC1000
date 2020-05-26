using Microsoft.EntityFrameworkCore.Migrations;

namespace MC1000.Migrations
{
    public partial class first2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliverySlot_DeliverySlot_DeliverySlotId",
                table: "DeliverySlot");

            migrationBuilder.DropIndex(
                name: "IX_DeliverySlot_DeliverySlotId",
                table: "DeliverySlot");

            migrationBuilder.DropColumn(
                name: "DeliverySlotId",
                table: "DeliverySlot");

            migrationBuilder.AddColumn<int>(
                name: "DeliverySlotId",
                table: "TimeSlot",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlot_DeliverySlotId",
                table: "TimeSlot",
                column: "DeliverySlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlot_DeliverySlot_DeliverySlotId",
                table: "TimeSlot",
                column: "DeliverySlotId",
                principalTable: "DeliverySlot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlot_DeliverySlot_DeliverySlotId",
                table: "TimeSlot");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlot_DeliverySlotId",
                table: "TimeSlot");

            migrationBuilder.DropColumn(
                name: "DeliverySlotId",
                table: "TimeSlot");

            migrationBuilder.AddColumn<int>(
                name: "DeliverySlotId",
                table: "DeliverySlot",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliverySlot_DeliverySlotId",
                table: "DeliverySlot",
                column: "DeliverySlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverySlot_DeliverySlot_DeliverySlotId",
                table: "DeliverySlot",
                column: "DeliverySlotId",
                principalTable: "DeliverySlot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
