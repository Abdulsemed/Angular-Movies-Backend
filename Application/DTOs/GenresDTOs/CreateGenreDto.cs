using Domain.validations;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.GenresDTOs;

public class CreateGenreDto
{
    [Required]
    [StringLength(100)]
    [FirstLetterUpperCase]
    public string Name { get; set; }
}
