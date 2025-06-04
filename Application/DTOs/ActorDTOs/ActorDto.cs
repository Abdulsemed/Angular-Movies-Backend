namespace Application.DTOs.ActorDTOs;
public class ActorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Biography { get; set; }
    public string? Picture { get; set; }
}
