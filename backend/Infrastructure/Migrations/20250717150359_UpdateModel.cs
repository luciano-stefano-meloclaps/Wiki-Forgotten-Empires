using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Ages_AgeId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Civilizations_CivilizationId",
                table: "Characters");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Ages_AgeId",
                table: "Characters",
                column: "AgeId",
                principalTable: "Ages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Civilizations_CivilizationId",
                table: "Characters",
                column: "CivilizationId",
                principalTable: "Civilizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Ages_AgeId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Civilizations_CivilizationId",
                table: "Characters");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Ages_AgeId",
                table: "Characters",
                column: "AgeId",
                principalTable: "Ages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Civilizations_CivilizationId",
                table: "Characters",
                column: "CivilizationId",
                principalTable: "Civilizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}