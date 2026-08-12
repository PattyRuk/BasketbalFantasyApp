using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasketbalFantasyApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class LinkTournamentToPlayerStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TournamentId",
                table: "PlayerStats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_TournamentId",
                table: "PlayerStats",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Tournaments_TournamentId",
                table: "PlayerStats",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Tournaments_TournamentId",
                table: "PlayerStats");

            migrationBuilder.DropIndex(
                name: "IX_PlayerStats_TournamentId",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "PlayerStats");
        }
    }
}
