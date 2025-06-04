using Application.DTOs.GenresDTOs;
using Application.DTOs.MovieTheaterDTOs;

namespace Application.DTOs.MoviesDTOs;
public class MoviesPostGetDto
{
    public List<GenreDto> Genres { get; set; }
    public List<MovieTheatersDto> MovieTheaters { get; set; }
}
