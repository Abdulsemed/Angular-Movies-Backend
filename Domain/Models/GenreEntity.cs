using Domain.Models.Common;
using Domain.validations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class GenreEntity : BaseEntity
{
    [Required]
    [StringLength(100)]
    [FirstLetterUpperCase]
    public string Name { get; set; }
}
