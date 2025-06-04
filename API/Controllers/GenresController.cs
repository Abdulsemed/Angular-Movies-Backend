using Application.DTOs.GenresDTOs;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]

    public class GenresController : ControllerBase
    {
        // GET: api/<GenresController>
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public GenresController(AppDbContext appDbContext, IMapper mapper)
        {
            _context = appDbContext;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public  async Task<ActionResult<List<GenreDto>>> GetGenres()
        {
            List<GenreEntity> genres = await _context.Generes.OrderBy(genre=>genre.Name).ToListAsync();
            var genresDto = _mapper.Map<List<GenreDto>>(genres);

            return StatusCode(200, genresDto);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<GenreDto>> GetGenresById(Guid Id)
        {
            var genres = await _context.Generes.FindAsync(Id);
            return StatusCode(200, genres);
        }

        [HttpPut]
        public async Task<ActionResult<GenreDto>> UpdateGenre(Guid Id, [FromBody] CreateGenreDto genreDto)
        {
            var genre = await _context.Generes.FindAsync(Id);
            if(genre == null)
            {
                await _context.AddAsync(genreDto);
                await _context.SaveChangesAsync();
            }
            else
            {
                genre.Name = genreDto.Name;
                _context.Generes.Update(genre);
                await _context.SaveChangesAsync();
            }
            
             return StatusCode(200, genre);
            

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteGenre(Guid Id)
        {
            var genre = await _context.Generes.FindAsync(Id);
            if (genre != null)
            {
                _context.Generes.Remove(genre);
                await _context.SaveChangesAsync();
                return StatusCode(204);
            }
            return StatusCode(404);
            
        }

        [HttpPost]
        public async Task<ActionResult<GenreEntity>> CreateGenre([FromBody] CreateGenreDto createGenreDto)
        {
            var genreEntity = _mapper.Map<GenreEntity>(createGenreDto);
            await _context.AddAsync(genreEntity);
            await _context.SaveChangesAsync();

            return StatusCode(200, genreEntity);
        }

    }
}
