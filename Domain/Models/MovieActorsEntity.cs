using Domain.Models.Common;

namespace Domain.Models;
public class MovieActorsEntity
{
    public string Character {  get; set; }
    public int Order {  get; set; }
    public Guid ActorId { get; set; }
    public Guid MovieId { get; set; }
    public MovieEntity Movie { get; set; }
    public ActorEntity Actor { get; set; }

}
