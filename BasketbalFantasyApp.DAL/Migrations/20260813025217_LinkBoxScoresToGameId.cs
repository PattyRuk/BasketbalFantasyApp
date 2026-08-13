using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasketbalFantasyApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class LinkBoxScoresToGameId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "PlayerStats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_GameId",
                table: "PlayerStats",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Games_GameId",
                table: "PlayerStats",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Games_GameId",
                table: "PlayerStats");

            migrationBuilder.DropIndex(
                name: "IX_PlayerStats_GameId",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "PlayerStats");
        }
    }
}
