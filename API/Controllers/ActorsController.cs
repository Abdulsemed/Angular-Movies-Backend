using API.Middlewares;
using Application.DTOs;
using Application.DTOs.ActorDTOs;
using Application.Responses;
using Application.Services.CloudinaryService;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
    public class ActorsController(ILogger<ActorsController> logger, AppDbContext context, IMapper mapper, ICloudinaryService cloudinaryService, StringValidator stringValidator) : ControllerBase
    {
        private readonly ILogger<ActorsController> _logger = logger;
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly StringValidator _stringValidator = stringValidator;


        [HttpGet("GetAllActors")]
        public async Task<ActionResult<PaginatedResponse>> GetAllActors([FromQuery] PaginationDto paginationDto)
        {
            var queryable = _context.Actors.AsQueryable();
            var actors = await queryable.Paginate(paginationDto).OrderBy(actor=>actor.Name).ToListAsync();
            var actorsDto = _mapper.Map<List<ActorDto>>(actors);
            var paginatedResponse = new PaginatedResponse()
            {
                TotalPages = await queryable.CountAsync(),
                Success = true,
                PageSize = paginationDto.PageSize,
                CurrentPage = paginationDto.CurrentPage,
                Message = "Actors fetched successfully",
                ReturnedObject = actorsDto
            };

            return StatusCode(200, paginatedResponse);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ActorDto>> GetActorById(Guid Id)
        {
            var actor = await _context.Actors.FindAsync(Id);
            return StatusCode(200, actor);
        }

        [HttpGet]
        [Route("SearchActors")]
        public async Task<ActionResult<List<ActorMovieDto>>> SearchByName(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name) || Name == "")
            {
                return StatusCode(200);
            }

            var searchedActors = await _context.Actors.Where(actor => actor.Name.Contains(Name)).OrderBy(actor => actor.Name).Take(5).ToListAsync();
            var searchedActorsDto = _mapper.Map<List<ActorMovieDto>>(searchedActors);
            return StatusCode(200, searchedActorsDto);
        }

        [HttpPost]
        public async Task<ActionResult<ActorDto>> CreateActor([FromForm] CreateActorDto createActorDto)
        {

            var actor = _mapper.Map<ActorEntity>(createActorDto);
            if(createActorDto.Picture != null)
            {
                var cloudResponse = await _cloudinaryService.UploadImageAsync(createActorDto.Picture);
                if (!cloudResponse.Success)
                {
                    return StatusCode(402, cloudResponse);
                };

                actor.Picture = cloudResponse.Link;

            }
            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();
            return StatusCode(200, actor);
        }

        [HttpPut]
        public async Task<ActionResult<ActorDto>> UpdateActor(Guid Id, [FromForm] CreateActorDto createActorDto)
        {
            var actor = await _context.Actors.FindAsync(Id);
            if (actor == null)
            {
                actor = _mapper.Map<ActorEntity>(createActorDto);
                if (actor.Picture != null)
                {
                    var cloudResponse = await _cloudinaryService.UploadImageAsync(createActorDto.Picture);
                    if (!cloudResponse.Success)
                    {
                        return StatusCode(402, cloudResponse);
                    };

                    actor.Picture = cloudResponse.Link;

                }
                await _context.Actors.AddAsync(actor);
                await _context.SaveChangesAsync();
            }
            else
            {
                var picture = actor.Picture;
                _mapper.Map(createActorDto, actor);
                Console.WriteLine(actor.Picture);
                if (createActorDto.Picture != null)
                {
                    if (picture != null)
                    {
                        var publicId = _stringValidator.GetLastIndexValue(picture);
                        var deleteResponse = await _cloudinaryService.DeleteFile(publicId);
                    }
                    var cloudResponse = await _cloudinaryService.UploadImageAsync(createActorDto.Picture);
                    if (!cloudResponse.Success)
                    {
                        return StatusCode(402, cloudResponse);
                    };

                    actor.Picture = cloudResponse.Link;

                }
                _context.Actors.Update(actor);
                await _context.SaveChangesAsync();
                actor = await _context.Actors.FindAsync(actor.Id);
            }

            return StatusCode(200, actor);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteActor(Guid Id)
        {
            var actor = await _context.Actors.FindAsync(Id);
            if (actor != null)
            {
                if(actor.Picture != null)
                {
                    var publicId = _stringValidator.GetLastIndexValue(actor.Picture);
                    var deleteResponse = await _cloudinaryService.DeleteFile(publicId);
                    Console.WriteLine(deleteResponse.Success);
                }
                _context.Remove(actor);
                await _context.SaveChangesAsync();
            }

            return StatusCode(204);
        }
    }
}
