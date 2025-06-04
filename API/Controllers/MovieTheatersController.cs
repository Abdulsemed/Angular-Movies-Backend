using Application.DTOs.MovieTheaterDTOs;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]

    public class MovieTheatersController(AppDbContext appDbContext, IMapper mapper) : ControllerBase
    {
        private readonly AppDbContext _context = appDbContext;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<MovieTheatersDto>>> GetAll()
        {
            var movieTheaters = await _context.MovieTheaters.ToListAsync();
            var movieTheatersDto = _mapper.Map<List<MovieTheatersDto>>(movieTheaters);
            return StatusCode(200, movieTheatersDto);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<MovieTheatersDto>> GetById(Guid Id)
        {
            var movieTheater = await _context.MovieTheaters.FindAsync(Id);
            var movieTheaterDto = _mapper.Map<MovieTheatersDto>(movieTheater);

            return StatusCode(200, movieTheaterDto);
        }

        [HttpPost]
        public async Task<ActionResult<MovieTheatersDto>> Create([FromBody] CreateMovieTheaterDto movieTheaterDto)
        {
            var movieTheater = _mapper.Map<MovieTheaterEntity>(movieTheaterDto);
            await _context.AddAsync(movieTheater);
            await _context.SaveChangesAsync();
            var movieTheaterdto = _mapper.Map<MovieTheatersDto>(movieTheater);
            return StatusCode(201, movieTheaterdto);
        }

        [HttpPut]
        public async Task<ActionResult<MovieTheatersDto>> Update(Guid Id, [FromBody] CreateMovieTheaterDto createMovieTheaterDto)
        {
            var movieTheater = await _context.MovieTheaters.FindAsync(Id);
            if(movieTheater == null)
            {
                movieTheater = _mapper.Map<MovieTheaterEntity>(createMovieTheaterDto);
                await _context.AddAsync(movieTheater);
                await _context.SaveChangesAsync();
            }
            else
            {
                movieTheater = _mapper.Map(createMovieTheaterDto, movieTheater);
                _context.Update(movieTheater);
                await _context.SaveChangesAsync();

            }

            var movieTheaterDto = _mapper.Map<MovieTheatersDto>(movieTheater);

            return StatusCode(200, movieTheaterDto);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid Id)
        {
            var movieTheater = await _context.MovieTheaters.FindAsync(Id);
            if (movieTheater != null)
            {
                _context.MovieTheaters.Remove(movieTheater);
                await _context.SaveChangesAsync();
                return StatusCode(204);
            }
            return StatusCode(404);
        }
    }
}
