using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTerritoryAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Territory",
                table: "Civilizations");

            migrationBuilder.CreateTable(
                name: "Territories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Territories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CivilizationTerritories",
                columns: table => new
                {
                    TerritoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CivilizationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CivilizationTerritories", x => new { x.CivilizationId, x.TerritoryId });
                    table.ForeignKey(
                        name: "FK_CivilizationTerritories_Civilizations_CivilizationId",
                        column: x => x.CivilizationId,
                        principalTable: "Civilizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CivilizationTerritories_Territories_TerritoryId",
                        column: x => x.TerritoryId,
                        principalTable: "Territories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CivilizationTerritories_TerritoryId",
                table: "CivilizationTerritories",
                column: "TerritoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CivilizationTerritories");

            migrationBuilder.DropTable(
                name: "Territories");

            migrationBuilder.AddColumn<int>(
                name: "Territory",
                table: "Civilizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
