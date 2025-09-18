using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCivilizationAgeJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CivilizationBattles_Battles_BattleId",
                table: "CivilizationBattles");

            migrationBuilder.DropForeignKey(
                name: "FK_CivilizationBattles_Civilizations_CivilizationId",
                table: "CivilizationBattles");

            migrationBuilder.DropForeignKey(
                name: "FK_CivilizationPeriods_Ages_AgeId",
                table: "CivilizationPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK_CivilizationPeriods_Civilizations_CivilizationId",
                table: "CivilizationPeriods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CivilizationPeriods",
                table: "CivilizationPeriods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CivilizationBattles",
                table: "CivilizationBattles");

            migrationBuilder.RenameTable(
                name: "CivilizationPeriods",
                newName: "CivilizationAge");

            migrationBuilder.RenameTable(
                name: "CivilizationBattles",
                newName: "CivilizationBattle");

            migrationBuilder.RenameIndex(
                name: "IX_CivilizationPeriods_AgeId",
                table: "CivilizationAge",
                newName: "IX_CivilizationAge_AgeId");

            migrationBuilder.RenameIndex(
                name: "IX_CivilizationBattles_BattleId",
                table: "CivilizationBattle",
                newName: "IX_CivilizationBattle_BattleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CivilizationAge",
                table: "CivilizationAge",
                columns: new[] { "CivilizationId", "AgeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CivilizationBattle",
                table: "CivilizationBattle",
                columns: new[] { "CivilizationId", "BattleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CivilizationAge_Ages_AgeId",
                table: "CivilizationAge",
                column: "AgeId",
                principalTable: "Ages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CivilizationAge_Civilizations_CivilizationId",
                table: "CivilizationAge",
                column: "CivilizationId",
                principalTable: "Civilizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CivilizationBattle_Battles_BattleId",
                table: "CivilizationBattle",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CivilizationBattle_Civilizations_CivilizationId",
                table: "CivilizationBattle",
                column: "CivilizationId",
                principalTable: "Civilizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CivilizationAge_Ages_AgeId",
                table: "CivilizationAge");

            migrationBuilder.DropForeignKey(
                name: "FK_CivilizationAge_Civilizations_CivilizationId",
                table: "CivilizationAge");

            migrationBuilder.DropForeignKey(
                name: "FK_CivilizationBattle_Battles_BattleId",
                table: "CivilizationBattle");

            migrationBuilder.DropForeignKey(
                name: "FK_CivilizationBattle_Civilizations_CivilizationId",
                table: "CivilizationBattle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CivilizationBattle",
                table: "CivilizationBattle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CivilizationAge",
                table: "CivilizationAge");

            migrationBuilder.RenameTable(
                name: "CivilizationBattle",
                newName: "CivilizationBattles");

            migrationBuilder.RenameTable(
                name: "CivilizationAge",
                newName: "CivilizationPeriods");

            migrationBuilder.RenameIndex(
                name: "IX_CivilizationBattle_BattleId",
                table: "CivilizationBattles",
                newName: "IX_CivilizationBattles_BattleId");

            migrationBuilder.RenameIndex(
                name: "IX_CivilizationAge_AgeId",
                table: "CivilizationPeriods",
                newName: "IX_CivilizationPeriods_AgeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CivilizationBattles",
                table: "CivilizationBattles",
                columns: new[] { "CivilizationId", "BattleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CivilizationPeriods",
                table: "CivilizationPeriods",
                columns: new[] { "CivilizationId", "AgeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CivilizationBattles_Battles_BattleId",
                table: "CivilizationBattles",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CivilizationBattles_Civilizations_CivilizationId",
                table: "CivilizationBattles",
                column: "CivilizationId",
                principalTable: "Civilizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CivilizationPeriods_Ages_AgeId",
                table: "CivilizationPeriods",
                column: "AgeId",
                principalTable: "Ages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CivilizationPeriods_Civilizations_CivilizationId",
                table: "CivilizationPeriods",
                column: "CivilizationId",
                principalTable: "Civilizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}