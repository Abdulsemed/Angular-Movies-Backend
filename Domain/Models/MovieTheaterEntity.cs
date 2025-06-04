using Domain.Models.Common;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
namespace Domain.Models;
public class MovieTheaterEntity : BaseEntity
{
    [Required]
    [StringLength(maximumLength:75)]
    public string Name { get; set; }
    public Point Location { get; set; }
}
