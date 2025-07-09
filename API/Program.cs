using API;
using API.Filters;
using API.Middlewares;
using Application.Mapping_Profiles;
using Application.Services.CloudinaryService;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
var builder = WebApplication.CreateBuilder(args);


AddSwaggerDoc(builder.Services);

DotNetEnv.Env.Load("../.env");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString: Environment.GetEnvironmentVariable("CONNECTION_STRING"), o => o.UseNetTopologySuite());
});

builder.Services.AddSingleton<ICloudinaryService, CloudinaryService>();
builder.Services.AddSingleton<StringValidator>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(MyExceptionFilter));
});

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddSingleton(provider => new MapperConfiguration(config =>
{
    var geometryFactory = provider.GetRequiredService<GeometryFactory>();
    config.AddProfile(new MappingProfiles(geometryFactory));
}).CreateMapper());

builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));


builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SIGNING_KEY"))),
        ClockSkew = TimeSpan.Zero
    };

    options.MapInboundClaims = false;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("isAdmin", policy => policy.RequireClaim("role", "admin"));
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();
app.UseCors(options =>
{
    //options.WithOrigins(Environment.GetEnvironmentVariable("FRONT_END_URL")).AllowAnyHeader().AllowAnyMethod();
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

// Add swagger documentation
void AddSwaggerDoc(IServiceCollection services)
{
services.AddSwaggerGen(c =>
{
c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
{
Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
Name = "Authorization",
In = ParameterLocation.Header,
Type = SecuritySchemeType.ApiKey,
Scheme = "Bearer"
});
c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
c.SwaggerDoc("v1", new OpenApiInfo
{
Version = "v1",
Title = "Spark Tank",
});
});
}

public partial class Program { }

