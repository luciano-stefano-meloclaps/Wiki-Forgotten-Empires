using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Civilizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Territory = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Civilizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    Outcome = table.Column<int>(type: "INTEGER", nullable: false),
                    Territory = table.Column<int>(type: "INTEGER", nullable: false),
                    AgeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Battles_Ages_AgeId",
                        column: x => x.AgeId,
                        principalTable: "Ages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Dynasty = table.Column<string>(type: "TEXT", nullable: false),
                    LifePeriod = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    CivilizationId = table.Column<int>(type: "INTEGER", nullable: false),
                    AgeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Ages_AgeId",
                        column: x => x.AgeId,
                        principalTable: "Ages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Civilizations_CivilizationId",
                        column: x => x.CivilizationId,
                        principalTable: "Civilizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CivilizationPeriods",
                columns: table => new
                {
                    AgeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CivilizationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CivilizationPeriods", x => new { x.CivilizationId, x.AgeId });
                    table.ForeignKey(
                        name: "FK_CivilizationPeriods_Ages_AgeId",
                        column: x => x.AgeId,
                        principalTable: "Ages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CivilizationPeriods_Civilizations_CivilizationId",
                        column: x => x.CivilizationId,
                        principalTable: "Civilizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CivilizationBattles",
                columns: table => new
                {
                    BattleId = table.Column<int>(type: "INTEGER", nullable: false),
                    CivilizationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CivilizationBattles", x => new { x.CivilizationId, x.BattleId });
                    table.ForeignKey(
                        name: "FK_CivilizationBattles_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CivilizationBattles_Civilizations_CivilizationId",
                        column: x => x.CivilizationId,
                        principalTable: "Civilizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterBattles",
                columns: table => new
                {
                    BattleId = table.Column<int>(type: "INTEGER", nullable: false),
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: false),
                    FactionName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterBattles", x => new { x.CharacterId, x.BattleId });
                    table.ForeignKey(
                        name: "FK_CharacterBattles_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterBattles_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Battles_AgeId",
                table: "Battles",
                column: "AgeId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterBattles_BattleId",
                table: "CharacterBattles",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_AgeId",
                table: "Characters",
                column: "AgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CivilizationId",
                table: "Characters",
                column: "CivilizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CivilizationBattles_BattleId",
                table: "CivilizationBattles",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_CivilizationPeriods_AgeId",
                table: "CivilizationPeriods",
                column: "AgeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterBattles");

            migrationBuilder.DropTable(
                name: "CivilizationBattles");

            migrationBuilder.DropTable(
                name: "CivilizationPeriods");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "Civilizations");

            migrationBuilder.DropTable(
                name: "Ages");
        }
    }
}