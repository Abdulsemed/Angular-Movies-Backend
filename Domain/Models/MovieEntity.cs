using Domain.Models.Common;

namespace Domain.Models;
public class MovieEntity : BaseEntity
{
    public string Title { get; set; }
    public string? Summary { get; set; }
    public string? Poster { get; set; }
    public string? Trailer { get; set; }
    public bool? InTheaters { get; set; }
    public DateTime? ReleaseDate {  get; set; }
    public List<MovieTheaterMovieEntity>? MovieTheaterMovies { get; set; }
    public List<MovieActorsEntity>? MovieActors { get; set; }
    public List<MovieGenres>? MoviesGenres { get; set; }
}
