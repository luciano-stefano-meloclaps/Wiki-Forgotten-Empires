using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncAfterRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BattleId",
                table: "Ages");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Ages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BattleId",
                table: "Ages",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "Ages",
                type: "INTEGER",
                nullable: true);
        }
    }
}
