using Microsoft.EntityFrameworkCore.Migrations;

namespace MC1000.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanner",
                table: "HomeBanner");

            migrationBuilder.CreateTable(
                name: "CategorieBanner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(nullable: true),
                    Afbeelding = table.Column<string>(nullable: true),
                    AfbeeldingUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorieBanner", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategorieBanner");

            migrationBuilder.AddColumn<bool>(
                name: "IsBanner",
                table: "HomeBanner",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
