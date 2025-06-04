using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class MovieV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Poster = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trailer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InTheaters = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GenreEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MovieTheaterEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Generes_GenreEntityId",
                        column: x => x.GenreEntityId,
                        principalTable: "Generes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Movies_MovieTheaters_MovieTheaterEntityId",
                        column: x => x.MovieTheaterEntityId,
                        principalTable: "MovieTheaters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MovieGenres",
                columns: table => new
                {
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenres", x => new { x.MovieId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MovieGenres_Movies_MovieEntityId",
                        column: x => x.MovieEntityId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MoviesActors",
                columns: table => new
                {
                    ActorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Character = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ActorEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MovieEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesActors", x => new { x.MovieId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_MoviesActors_Actors_ActorEntityId",
                        column: x => x.ActorEntityId,
                        principalTable: "Actors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MoviesActors_Movies_MovieEntityId",
                        column: x => x.MovieEntityId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MovieTheaterMovies",
                columns: table => new
                {
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieTheaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieTheaterMovies", x => new { x.MovieId, x.MovieTheaterId });
                    table.ForeignKey(
                        name: "FK_MovieTheaterMovies_Movies_MovieEntityId",
                        column: x => x.MovieEntityId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_MovieEntityId",
                table: "MovieGenres",
                column: "MovieEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenreEntityId",
                table: "Movies",
                column: "GenreEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MovieTheaterEntityId",
                table: "Movies",
                column: "MovieTheaterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesActors_ActorEntityId",
                table: "MoviesActors",
                column: "ActorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesActors_MovieEntityId",
                table: "MoviesActors",
                column: "MovieEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieTheaterMovies_MovieEntityId",
                table: "MovieTheaterMovies",
                column: "MovieEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieGenres");

            migrationBuilder.DropTable(
                name: "MoviesActors");

            migrationBuilder.DropTable(
                name: "MovieTheaterMovies");


            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
