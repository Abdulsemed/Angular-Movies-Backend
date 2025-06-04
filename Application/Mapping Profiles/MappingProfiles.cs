using Application.DTOs.ActorDTOs;
using Application.DTOs.Auth;
using Application.DTOs.GenresDTOs;
using Application.DTOs.MoviesDTOs;
using Application.DTOs.MovieTheaterDTOs;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;

namespace Application.Mapping_Profiles;
public class MappingProfiles : Profile
{
    public MappingProfiles(GeometryFactory geometryFactory)
    {
        #region Genre Entity 
        #endregion
        CreateMap<CreateGenreDto, GenreEntity>().ReverseMap();
        CreateMap<GenreDto, GenreEntity>().ReverseMap();

        #region ActorEntity
        #endregion
        CreateMap<ActorDto, ActorEntity>().ReverseMap();
        CreateMap<CreateActorDto, ActorEntity>().ForMember(actor=>actor.Picture, options=>options.Ignore());
        CreateMap<ActorMovieDto, ActorEntity>().ReverseMap();

        #region MovieTheaters Entity
        #endregion
        CreateMap<MovieTheaterEntity, MovieTheatersDto>()
            .ForMember(movieTheater => movieTheater.Latitude, dto => dto.MapFrom(prop => prop.Location.Y))
            .ForMember(movieTheater => movieTheater.Longitude, dto => dto.MapFrom(prop => prop.Location.X));

        CreateMap<CreateMovieTheaterDto, MovieTheaterEntity>()
            .ForMember(movieTheaterEntity => movieTheaterEntity.Location, dto => dto.MapFrom(prop => geometryFactory.CreatePoint( new Coordinate(prop.Longitude, prop.Latitude))));

        #region Movie Entity 
        #endregion
        CreateMap<CreateMovieDto, MovieEntity>()
            .ForMember(dto => dto.Poster, options => options.Ignore())
            .ForMember(dto => dto.MovieActors, options => options.MapFrom(MapMovieActors))
            .ForMember(dto => dto.MovieTheaterMovies, options => options.MapFrom(MapMovieTheaterMovies))
            
            .ForMember(dto => dto.MoviesGenres, options => options.MapFrom(MapMovieGenres));

        CreateMap<MovieEntity, MovieDto>()
            .ForMember(dto => dto.Genres, options => options.MapFrom(MapMoviesGenres))
            .ForMember(dto => dto.MovieTheaters, options => options.MapFrom(MapMovieTheatersGenres))
            .ForMember(dto => dto.Actors, options => options.MapFrom(MapMoviesActors));

        #region User Entity
        #endregion
        CreateMap<IdentityUser, UserDto>();
    }

    private List<MovieActorDto> MapMoviesActors(MovieEntity movie, MovieDto movieDto)
    {
        var list = new List<MovieActorDto>();
        if(movie.MovieActors != null)
        {
            foreach(var actor in movie.MovieActors)
            {
                list.Add(new MovieActorDto() { Id = actor.ActorId, Character = actor.Character, Name = actor.Actor.Name, Order = actor.Order, Picture = actor.Actor.Picture });
            }
        }

        return list;
    }

    private List<MovieTheatersDto> MapMovieTheatersGenres(MovieEntity movie, MovieDto movieDto)
    {
        var list = new List<MovieTheatersDto>();
        if(movie.MovieTheaterMovies != null)
        {
            foreach(var movieTheater in movie.MovieTheaterMovies)
            {
                list.Add(new MovieTheatersDto() { Id = movieTheater.MovieTheaterId, Name = movieTheater.MovieTheater.Name, Latitude = movieTheater.MovieTheater.Location.Y, Longitude = movieTheater.MovieTheater.Location.X });
            }
        }
        return list;
    }
    private List<GenreDto> MapMoviesGenres(MovieEntity movie, MovieDto movieDto)
    {
        var list = new List<GenreDto>();
        if( movie.MoviesGenres != null )
        {
            foreach(var genre in movie.MoviesGenres)
            {
                list.Add(new GenreDto { Id = genre.GenreId, Name = genre.Genre.Name });
            }
        }

        return list;
    }

    private List<MovieTheaterMovieEntity> MapMovieTheaterMovies(CreateMovieDto createMovieDto, MovieEntity movie) 
    {
        var list = new List<MovieTheaterMovieEntity>();
        if( createMovieDto.MovieTheaterIds.Count  > 0 )
        {
            foreach(var id in  createMovieDto.MovieTheaterIds)
            {
                list.Add(new MovieTheaterMovieEntity() { MovieTheaterId = id });
            };
        };

        return list;

    }

    private List<MovieGenres> MapMovieGenres(CreateMovieDto createMovieDto, MovieEntity movie)
    {
        var list = new List<MovieGenres>();
        if(createMovieDto.GenreIds.Count > 0)
        {
            foreach(var id in createMovieDto.GenreIds)
            {
                list.Add(new MovieGenres() {  GenreId = id});
            };
        };
        return list;
    }

    private List<MovieActorsEntity> MapMovieActors(CreateMovieDto createMovieDto, MovieEntity movie)
    {
        var list = new List<MovieActorsEntity>();
        if(createMovieDto.Actors.Count > 0)
        {
            foreach(var actor in createMovieDto.Actors)
            {
                list.Add(new MovieActorsEntity() { ActorId = actor.ActorId, Character = actor.Character});
            };
        };

        return list;
    }
}
