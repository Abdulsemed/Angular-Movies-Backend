using Application.DTOs.GenresDTOs;

namespace Application.DTOs.MoviesDTOs;
public class FilterPaginationDto
{
    public string? Title { get; set; }
    public bool? UpcomingReleases { get; set; }
    public bool? InTheaters { get; set; }
    public Guid? Genre { get; set; }
    public PaginationDto Pagination { get; set; }
}
