using Domain.Models.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
public class RatingEntity : BaseEntity
{
    [Range(1,5)]
    public int Rating { get; set; }
    public Guid MovieId { get; set; }
    public MovieEntity Movie { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
}
