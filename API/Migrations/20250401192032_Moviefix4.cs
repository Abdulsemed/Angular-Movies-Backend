using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Moviefix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MoviesActors");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MoviesActors");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MoviesActors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors",
                columns: new[] { "MovieId", "ActorId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "MoviesActors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MoviesActors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MoviesActors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors",
                columns: new[] { "MovieId", "ActorId", "Id" });
        }
    }
}
