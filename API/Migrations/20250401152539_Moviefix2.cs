using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Moviefix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieEntityId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesActors_Movies_MovieEntityId",
                table: "MoviesActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieTheaterMovies_Movies_MovieEntityId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropIndex(
                name: "IX_MovieTheaterMovies_MovieEntityId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropIndex(
                name: "IX_MoviesActors_MovieEntityId",
                table: "MoviesActors");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_MovieEntityId",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "MovieEntityId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropColumn(
                name: "MovieEntityId",
                table: "MoviesActors");

            migrationBuilder.DropColumn(
                name: "MovieEntityId",
                table: "MovieGenres");

            migrationBuilder.CreateIndex(
                name: "IX_MovieTheaterMovies_MovieTheaterId",
                table: "MovieTheaterMovies",
                column: "MovieTheaterId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesActors_ActorId",
                table: "MoviesActors",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Generes_GenreId",
                table: "MovieGenres",
                column: "GenreId",
                principalTable: "Generes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieId",
                table: "MovieGenres",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesActors_Actors_ActorId",
                table: "MoviesActors",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesActors_Movies_MovieId",
                table: "MoviesActors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieTheaterMovies_MovieTheaters_MovieTheaterId",
                table: "MovieTheaterMovies",
                column: "MovieTheaterId",
                principalTable: "MovieTheaters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieTheaterMovies_Movies_MovieId",
                table: "MovieTheaterMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Generes_GenreId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesActors_Actors_ActorId",
                table: "MoviesActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesActors_Movies_MovieId",
                table: "MoviesActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieTheaterMovies_MovieTheaters_MovieTheaterId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieTheaterMovies_Movies_MovieId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropIndex(
                name: "IX_MovieTheaterMovies_MovieTheaterId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropIndex(
                name: "IX_MoviesActors_ActorId",
                table: "MoviesActors");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres");

            migrationBuilder.AddColumn<Guid>(
                name: "MovieEntityId",
                table: "MovieTheaterMovies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MovieEntityId",
                table: "MoviesActors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MovieEntityId",
                table: "MovieGenres",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieTheaterMovies_MovieEntityId",
                table: "MovieTheaterMovies",
                column: "MovieEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesActors_MovieEntityId",
                table: "MoviesActors",
                column: "MovieEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_MovieEntityId",
                table: "MovieGenres",
                column: "MovieEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieEntityId",
                table: "MovieGenres",
                column: "MovieEntityId",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesActors_Movies_MovieEntityId",
                table: "MoviesActors",
                column: "MovieEntityId",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieTheaterMovies_Movies_MovieEntityId",
                table: "MovieTheaterMovies",
                column: "MovieEntityId",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
