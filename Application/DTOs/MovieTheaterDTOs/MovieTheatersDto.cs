using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MovieTheaterDTOs;
public class MovieTheatersDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}
