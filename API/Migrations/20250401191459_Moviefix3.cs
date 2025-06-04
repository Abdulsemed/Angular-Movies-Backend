using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Moviefix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors",
                columns: new[] { "MovieId", "ActorId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors",
                columns: new[] { "MovieId", "ActorId" });
        }
    }
}
