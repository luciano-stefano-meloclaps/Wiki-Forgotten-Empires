using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAgeBattleRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battles_Ages_AgeId",
                table: "Battles");

            migrationBuilder.AddForeignKey(
                name: "FK_Battles_Ages_AgeId",
                table: "Battles",
                column: "AgeId",
                principalTable: "Ages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battles_Ages_AgeId",
                table: "Battles");

            migrationBuilder.AddForeignKey(
                name: "FK_Battles_Ages_AgeId",
                table: "Battles",
                column: "AgeId",
                principalTable: "Ages",
                principalColumn: "Id");
        }
    }
}