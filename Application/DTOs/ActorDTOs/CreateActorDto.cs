using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.ActorDTOs;
public class CreateActorDto
{
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Biography { get; set; }
    public IFormFile? Picture { get; set; }
}
