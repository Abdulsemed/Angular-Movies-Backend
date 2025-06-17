using API.Middlewares;
using Application.DTOs;
using Application.DTOs.GenresDTOs;
using Application.DTOs.MoviesDTOs;
using Application.DTOs.MovieTheaterDTOs;
using Application.Responses;
using Application.Services.CloudinaryService;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
    public class MoviesController(AppDbContext appDbContext, IMapper mapper, ICloudinaryService cloudinaryService, StringValidator stringValidator, UserManager<IdentityUser> userManager) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly AppDbContext _context = appDbContext;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly StringValidator _stringValidator = stringValidator;
        private readonly UserManager<IdentityUser> _userManager = userManager;


        [HttpGet]
        [Route("GetMovieById")]
        [AllowAnonymous]
        public async Task<ActionResult<MovieDto>> GetMovieById(Guid Id)
        {
            var movie = await _context.Movies.Include(movie => movie.MoviesGenres).ThenInclude(m => m.Genre)
                .Include(movie => movie.MovieTheaterMovies).ThenInclude(m => m.MovieTheater)
                .Include(movie => movie.MovieActors).ThenInclude(m => m.Actor)
                .FirstOrDefaultAsync(m => m.Id == Id);

            if (movie == null)
            {
                return StatusCode(404);
            }

            double averageVote = 0;
            int userVote = 0;
            if(await _context.Ratings.AnyAsync(rate=> rate.MovieId == movie.Id))
            {
                averageVote = await _context.Ratings.Where(rate => rate.MovieId == movie.Id).AverageAsync(x => x.Rating);
                Console.WriteLine("is authenticated" + HttpContext.User.Identity.IsAuthenticated);
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    string email = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "email").Value;
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user != null) { 
                        var userRating = await _context.Ratings.FirstOrDefaultAsync(rating=> rating.UserId == user.Id && rating.MovieId == movie.Id);
                        if (userRating != null)
                        {
                            userVote = userRating.Rating;
                        }
                    }
                }
            }

            var movieDto = _mapper.Map<MovieDto>(movie);
            movieDto.AverageVote = averageVote;
            movieDto.UserVote = userVote;
            movieDto.Actors = [.. movieDto.Actors.OrderBy(actor => actor.Order)];
            return StatusCode(200, movieDto);
        }

        [HttpGet]
        [Route("GetMovieToUpdate")]
        public async Task<ActionResult<MoviePutGetDto>> GetMovieToUpdate(Guid Id)
        {
            var movieActionResult = await GetMovieById(Id);
            if (movieActionResult.Result is NotFoundResult)
            {
                Console.Write("here is it");
                return StatusCode(404);
            }

            var okResult = movieActionResult.Result as ObjectResult;
            var movie = okResult!.Value as MovieDto;

            var selectedGenreIds = movie.Genres.Select(genre => genre.Id).ToList();
            var nonSelectedGenres = await _context.Generes.Where(genre => !selectedGenreIds.Contains(genre.Id)).ToListAsync();

            var selectedMovieTheaters = movie.MovieTheaters.Select(movieTheater => movieTheater.Id).ToList();
            var nonSelectedMovieTheaters = await _context.MovieTheaters.Where(movieTheater => !selectedMovieTheaters.Contains(movieTheater.Id)).ToListAsync();

            var nonSelectedGenresDto = _mapper.Map<List<GenreDto>>(nonSelectedGenres);
            var nonSelectedMovieTheatersDto = _mapper.Map<List<MovieTheatersDto>>(nonSelectedMovieTheaters);

            var response = new MoviePutGetDto()
            {
                Movie = movie,
                NonSelectedGenres = nonSelectedGenresDto,
                NonSelectedMovieTheaters = nonSelectedMovieTheatersDto,
                SelectedGenres = movie.Genres,
                SelectedMovieTheaters = movie.MovieTheaters,
                Actors = movie.Actors
            };

            return StatusCode(200, response);
        }

        [HttpGet]
        [Route("FilterMovies")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PaginatedResponse>>> FilterMovies([FromQuery] FilterPaginationDto filterPagination)
        {
            var moviesQueryable = _context.Movies.AsQueryable();
            if (filterPagination.Title != null && !(filterPagination.Title == null))
            {
                moviesQueryable = moviesQueryable.Where(movie => movie.Title.Contains(filterPagination.Title));
            }

            if (filterPagination.UpcomingReleases == true)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(movie => movie.ReleaseDate > today);
            };

            if (filterPagination.InTheaters == true)
            {
                moviesQueryable = moviesQueryable.Where(movie => movie.InTheaters == true);
            };

            if (filterPagination.Genre != null)
            {
                moviesQueryable = moviesQueryable.Where(movie => movie.MoviesGenres.Any(genre => genre.GenreId == filterPagination.Genre));
            }

            int totalPages = moviesQueryable.Count();

            moviesQueryable = moviesQueryable.Take(filterPagination.Pagination.pageSize).Skip((filterPagination.Pagination.CurrentPage - 1) * filterPagination.Pagination.pageSize);
            List<MovieDto> movies = [];
            var movieList = moviesQueryable.ToList();
            Console.WriteLine(movieList.Count);
            foreach (var movie in movieList)
            {
                var movieDto = _mapper.Map<MovieDto>(movie);
                var genres = await _context.Generes.Where(genre => _context.MovieGenres
                        .Where(mg => mg.MovieId == movie.Id)
                        .Select(mg => mg.GenreId)
                        .Contains(genre.Id))
                    .ToListAsync();
                movieDto.Genres = _mapper.Map <List<GenreDto>> (genres);
                if (await _context.Ratings.AnyAsync(rate => rate.MovieId == movie.Id))
                {
                    double averageVote = await _context.Ratings.Where(rate => rate.MovieId == movie.Id).AverageAsync(x => x.Rating);
                    movieDto.AverageVote = averageVote;
                }
                    movies.Add(movieDto);

            }
            var response = new PaginatedResponse()
            {
                ReturnedObject = movies,
                Success = true,
                PageSize = filterPagination.Pagination.pageSize,
                CurrentPage = filterPagination.Pagination.CurrentPage,
                TotalPages = totalPages,
                Message = "Movies filtered successfully"
            };
            return StatusCode(200, response);
        }

        [HttpPut]
        public async Task<ActionResult<MovieDto>> UpdateMovie(Guid Id, [FromForm] CreateMovieDto createMovieDto)
        {
            var movie = await _context.Movies.Include(movie=>movie.MoviesGenres).Include(movie=>movie.MovieTheaterMovies).Include(movie=>movie.MovieActors).FirstOrDefaultAsync(movie=>movie.Id ==  Id);
            if(movie == null)
            {
                return StatusCode(404);
            }

            movie = _mapper.Map(createMovieDto, movie);
            if(createMovieDto.Poster != null)
            {
                if(movie.Poster != null)
                {
                    var publicId = _stringValidator.GetLastIndexValue(movie.Poster);
                    var deleteResponse = await _cloudinaryService.DeleteFile(publicId);
                    if (!deleteResponse.Success)
                    {
                        return StatusCode(400);
                    }

                }

                var cloudResponse = await _cloudinaryService.UploadImageAsync(createMovieDto.Poster);
                if(!cloudResponse.Success)
                {
                    return StatusCode(400);
                }

                movie.Poster = cloudResponse.Link;
            }
            AnotateActorsOrder(movie);
            _context.Update(movie);
            await _context.SaveChangesAsync();
            return StatusCode(200, movie);

        }

        [HttpGet]
        [Route("GetHomePage")]
        [AllowAnonymous]
        public async Task<ActionResult<HomeDto>> GetHomePage()
        {
            var top = 6;
            var today = DateTime.Today;
            var Intheater = await _context.Movies.Where(movie => movie.InTheaters == true).Take(top).ToListAsync();
            var upcomingReleases = await _context.Movies.Where(movie=> movie.ReleaseDate > today).Take(top).ToListAsync();

            var homeDto = new HomeDto()
            {
                InTheaters = _mapper.Map<List<MovieDto>>(upcomingReleases),
                UpcomingReleases = _mapper.Map<List<MovieDto>>(Intheater)
            };

            return StatusCode(200, homeDto);
        }

        [HttpGet]
        [Route("GetMoviePost")]
        public async Task<ActionResult<MoviesPostGetDto>> GetMoviePost()
        {
            var movieTheaters = await _context.MovieTheaters.OrderBy(movieTheater=>movieTheater.Name).ToListAsync();
            var genres = await _context.Generes.OrderBy(genre=>genre.Name).ToListAsync();

            var movieTheatersDto = _mapper.Map<List<MovieTheatersDto>>(movieTheaters);
            var genresDto = _mapper.Map<List<GenreDto>>(genres);

            var moviesPostDto = new MoviesPostGetDto() { Genres = genresDto, MovieTheaters = movieTheatersDto };
            return StatusCode(200, moviesPostDto);
        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> Create([FromForm] CreateMovieDto createMovieDto)
        {
            var movie = _mapper.Map<MovieEntity>(createMovieDto);
            if (createMovieDto.Poster != null)
            {
                var cloudResponse = await _cloudinaryService.UploadImageAsync(createMovieDto.Poster);
                if (!cloudResponse.Success)
                {
                    return StatusCode(402, cloudResponse);
                }
                movie.Poster = cloudResponse.Link;
            }
            AnotateActorsOrder(movie);
            await _context.AddAsync(movie);
            await _context.SaveChangesAsync();
            return StatusCode(200, movie);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovie(Guid Id)
        {
            var movie = await _context.Movies.FindAsync(Id);
            if (movie == null) {
                return StatusCode(404);
            }
            
            if(movie.Poster != null)
            {
                var publicId = _stringValidator.GetLastIndexValue(movie.Poster);
                var deleteResponse = await _cloudinaryService.DeleteFile(publicId);
                if (!deleteResponse.Success)
                {
                    return StatusCode(400);
                }
            }
            _context.Remove(movie);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private static void AnotateActorsOrder(MovieEntity movie)
        {
            if(movie.MovieActors != null)
            {
                for(int i= 0; i < movie.MovieActors.Count; i++)
                {
                    movie.MovieActors[i].Order = i;
                };
            };

        }
    }
}
