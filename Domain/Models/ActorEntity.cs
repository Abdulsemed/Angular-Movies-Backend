using Domain.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class ActorEntity : BaseEntity
{
    [Required]
    [StringLength(120)]
    public string Name {  get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Biography { get; set; }
    public string? Picture { get; set; }
}
