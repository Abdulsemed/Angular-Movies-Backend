namespace Domain.Models;
public class MovieGenres
{
    public Guid MovieId { get; set; }
    public Guid GenreId { get; set; }
    public GenreEntity Genre { get; set; }
    public MovieEntity Movie { get; set; }
}
