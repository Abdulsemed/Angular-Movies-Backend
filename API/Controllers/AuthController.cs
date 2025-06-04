using API.Middlewares;
using Application.DTOs;
using Application.DTOs.Auth;
using Application.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AuthController(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager, 
            IConfiguration configuration,
            AppDbContext context,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("ListUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
        public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] PaginationDto paginationDto)
        {
            var usersQuery = _context.Users.AsQueryable();
            var users = await usersQuery.Paginate(paginationDto).OrderBy(user=> user.Email).ToListAsync();
            var userDto = _mapper.Map<List<UserDto>>(users);
            PaginatedResponse paginatedResponse = new()
            {
                ReturnedObject = userDto,
                Success = true,
                Message = "Users fetched successfully",
                PageSize = paginationDto.PageSize,
                CurrentPage = paginationDto.CurrentPage,
                TotalPages = await usersQuery.CountAsync()
            };

            return StatusCode(200, paginatedResponse);
        }

        [HttpPost]
        [Route("MakeUserAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
        public async Task<IActionResult> MakeUserAdmin([FromBody]string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return StatusCode(404);
            }
            var claims = await _userManager.GetClaimsAsync(user);
            if (claims.FirstOrDefault(claim=> claim.Type == "role" && claim.Value == "admin") != null)
            {
                return StatusCode(400);
            }

            await _userManager.AddClaimAsync(user, new Claim("role", "admin"));

            return StatusCode(204);
        }

        [HttpPost]
        [Route("MakeAdminUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
        public async Task<IActionResult> MakeAdminUser([FromBody] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return StatusCode(404);
            }

            var claims = await _userManager.GetClaimsAsync(user);
            if (claims.FirstOrDefault(claim => claim.Type == "role" && claim.Value == "admin") == null)
            {
                return StatusCode(400);
            }

            await _userManager.RemoveClaimAsync(user, new Claim("role", "admin"));

            return StatusCode(204);
        }


        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<AuthenticationResponse>> CreateUser([FromBody] UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
            var result = await _userManager.CreateAsync(user, userCredentials.Password);
            if (result.Succeeded)
            {
                var authenticationResponse = await CreateToken(userCredentials);
                return StatusCode(201, authenticationResponse);
            }
            else
            {
                return StatusCode(422, result.Errors);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
        {
            var result = await _signInManager.PasswordSignInAsync(userName:userCredentials.Email, userCredentials.Password, isPersistent:false, lockoutOnFailure:false);
            if (result.Succeeded)
            {
                var authenticationResponse = await CreateToken(userCredentials);
                return StatusCode(200, authenticationResponse);
            }
            else
            {
                return StatusCode(422, "Email or password is incorrect");
            }
        }


        private async Task<AuthenticationResponse> CreateToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new("email", userCredentials.Email),
            };

            var user = await _userManager.FindByEmailAsync(userCredentials.Email);
            var dbClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(dbClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SIGNING_KEY")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(issuer:null, audience:null, claims:claims, expires:expiration, signingCredentials:creds);
            return new AuthenticationResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };

        }

    }
}
