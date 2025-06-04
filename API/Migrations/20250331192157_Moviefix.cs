using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Moviefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieEntityId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Generes_GenreEntityId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_MovieTheaters_MovieTheaterEntityId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesActors_Actors_ActorEntityId",
                table: "MoviesActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieTheaterMovies_Movies_MovieEntityId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropIndex(
                name: "IX_MovieTheaterMovies_MovieEntityId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropIndex(
                name: "IX_MoviesActors_ActorEntityId",
                table: "MoviesActors");

            migrationBuilder.DropIndex(
                name: "IX_Movies_GenreEntityId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_MovieTheaterEntityId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_MovieEntityId",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "MovieEntityId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropColumn(
                name: "ActorEntityId",
                table: "MoviesActors");

            migrationBuilder.DropColumn(
                name: "GenreEntityId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MovieTheaterEntityId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MovieEntityId",
                table: "MovieGenres");

            migrationBuilder.CreateIndex(
                name: "IX_MovieTheaterMovies_MovieTheaterId",
                table: "MovieTheaterMovies",
                column: "MovieTheaterId");

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
                name: "FK_MovieTheaterMovies_MovieTheaters_MovieTheaterId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieTheaterMovies_Movies_MovieId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropIndex(
                name: "IX_MovieTheaterMovies_MovieTheaterId",
                table: "MovieTheaterMovies");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres");

            migrationBuilder.AddColumn<Guid>(
                name: "MovieEntityId",
                table: "MovieTheaterMovies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActorEntityId",
                table: "MoviesActors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GenreEntityId",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MovieTheaterEntityId",
                table: "Movies",
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
                name: "IX_MoviesActors_ActorEntityId",
                table: "MoviesActors",
                column: "ActorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenreEntityId",
                table: "Movies",
                column: "GenreEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MovieTheaterEntityId",
                table: "Movies",
                column: "MovieTheaterEntityId");

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
                name: "FK_Movies_Generes_GenreEntityId",
                table: "Movies",
                column: "GenreEntityId",
                principalTable: "Generes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_MovieTheaters_MovieTheaterEntityId",
                table: "Movies",
                column: "MovieTheaterEntityId",
                principalTable: "MovieTheaters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesActors_Actors_ActorEntityId",
                table: "MoviesActors",
                column: "ActorEntityId",
                principalTable: "Actors",
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
