using Application.DTOs.RatingDTOs;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public RatingController(AppDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RatingEntity>> CreateOrUpdateRating([FromBody] RatingDto ratingDto)
        {
            Console.WriteLine(HttpContext.User.Claims.ToArray()[0]);
            var email = HttpContext.User.Claims.FirstOrDefault(token=> token.Type ==  "email").Value;
            Console.WriteLine("email is " + email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return StatusCode(401);
            }

            string userId = user.Id;
            var movie = await _context.Movies.FindAsync(ratingDto.MovieId);
            if (movie == null)
            {
                return StatusCode(404);
            }
            var currentRating = await _context.Ratings.FirstOrDefaultAsync(rating=> rating.UserId == userId && rating.MovieId == ratingDto.MovieId);
            if (currentRating == null)
            {
                currentRating = new RatingEntity()
                {
                    MovieId = ratingDto.MovieId,
                    UserId = userId,
                    Rating = ratingDto.Rating

                };

                _context.Add(currentRating);

            }
            else
            {
                currentRating.Rating = ratingDto.Rating;
            }

            await _context.SaveChangesAsync();

            return StatusCode(201, currentRating);
        }
    }
}
