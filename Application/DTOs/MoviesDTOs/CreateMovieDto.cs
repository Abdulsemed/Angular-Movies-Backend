using API.Middlewares;
using Application.DTOs.MovieActorDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.MoviesDTOs;
public class CreateMovieDto
{
    public string Title { get; set; }
    public string Summary { get; set; }
    public IFormFile? Poster { get; set; }
    public string Trailer { get; set; }
    public bool InTheaters { get; set; }
    public DateTime ReleaseDate { get; set; }
    [ModelBinder (binderType:typeof(TypeBinder<List<Guid>>))]
    public List<Guid> GenreIds { get; set; }
    [ModelBinder(binderType: typeof(TypeBinder<List<Guid>>))]
    public List<Guid> MovieTheaterIds { get; set; }
    [ModelBinder(binderType: typeof(TypeBinder<List<CreateMovieActorDto>>))]
    public List<CreateMovieActorDto> Actors { get; set; }
}
