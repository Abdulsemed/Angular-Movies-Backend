using Application.DTOs.ActorDTOs;
using Application.DTOs.GenresDTOs;
using Application.DTOs.MovieTheaterDTOs;

namespace Application.DTOs.MoviesDTOs;
public class MoviePutGetDto
{
    public MovieDto Movie { get; set; }
    public List<GenreDto> SelectedGenres { get; set; }
    public List<GenreDto> NonSelectedGenres { get; set; }
    public List<MovieTheatersDto> SelectedMovieTheaters { get; set; }
    public List<MovieTheatersDto> NonSelectedMovieTheaters { get; set; }
    public List<MovieActorDto> Actors { get; set; }
}
