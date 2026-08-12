using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasketbalFantasyApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class IntegrateTournamentGamesSimulation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FormatType",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Tournaments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MvpPlayerId",
                table: "Tournaments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequiredWins",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinnerTeamId",
                table: "Tournaments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TournamentTeamTeamId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TournamentTeamTournamentId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    TeamAId = table.Column<int>(type: "int", nullable: false),
                    TeamBId = table.Column<int>(type: "int", nullable: false),
                    TeamAScore = table.Column<int>(type: "int", nullable: false),
                    TeamBScore = table.Column<int>(type: "int", nullable: false),
                    MatchTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WinnerTeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_Teams_TeamAId",
                        column: x => x.TeamAId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Teams_TeamBId",
                        column: x => x.TeamBId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTeams",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    WinsCount = table.Column<int>(type: "int", nullable: false),
                    LossesCount = table.Column<int>(type: "int", nullable: false),
                    FinalPosition = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTeams", x => new { x.TournamentId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_TournamentTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentTeams_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_MvpPlayerId",
                table: "Tournaments",
                column: "MvpPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_WinnerTeamId",
                table: "Tournaments",
                column: "WinnerTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TournamentTeamTournamentId_TournamentTeamTeamId",
                table: "Players",
                columns: new[] { "TournamentTeamTournamentId", "TournamentTeamTeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamAId",
                table: "Games",
                column: "TeamAId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamBId",
                table: "Games",
                column: "TeamBId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TournamentId",
                table: "Games",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeams_TeamId",
                table: "TournamentTeams",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_TournamentTeams_TournamentTeamTournamentId_TournamentTeamTeamId",
                table: "Players",
                columns: new[] { "TournamentTeamTournamentId", "TournamentTeamTeamId" },
                principalTable: "TournamentTeams",
                principalColumns: new[] { "TournamentId", "TeamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Players_MvpPlayerId",
                table: "Tournaments",
                column: "MvpPlayerId",
                principalTable: "Players",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Teams_WinnerTeamId",
                table: "Tournaments",
                column: "WinnerTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_TournamentTeams_TournamentTeamTournamentId_TournamentTeamTeamId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Players_MvpPlayerId",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Teams_WinnerTeamId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "TournamentTeams");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_MvpPlayerId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_WinnerTeamId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Players_TournamentTeamTournamentId_TournamentTeamTeamId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "MvpPlayerId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "RequiredWins",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "WinnerTeamId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "TournamentTeamTeamId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TournamentTeamTournamentId",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "FormatType",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
