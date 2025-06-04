namespace Domain.Models;
public class MovieTheaterMovieEntity
{
    public Guid MovieId { get; set; }
    public Guid MovieTheaterId { get; set; }
    public MovieTheaterEntity MovieTheater { get; set; }
    public MovieEntity Movie { get; set; }
}
