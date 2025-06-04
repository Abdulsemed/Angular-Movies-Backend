using Application.DTOs.ActorDTOs;
using Application.DTOs.GenresDTOs;
using Application.DTOs.MovieTheaterDTOs;

namespace Application.DTOs.MoviesDTOs;
public class MovieDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Poster { get; set; }
    public string Trailer { get; set; }
    public bool InTheaters { get; set; }
    public double AverageVote {  get; set; }
    public int UserVote {  get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<GenreDto> Genres { get; set; }
    public List<MovieTheatersDto> MovieTheaters { get; set; }
    public List<MovieActorDto> Actors { get; set; }
}
