using Domain.Models;
using Domain.Models.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace API;
public class AppDbContext : IdentityDbContext
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieGenres>().HasKey(x => new { x.MovieId, x.GenreId });
        modelBuilder.Entity<MovieTheaterMovieEntity>().HasKey(x => new { x.MovieId,x.MovieTheaterId});
        modelBuilder.Entity<MovieActorsEntity>().HasKey(x => new { x.MovieId, x.ActorId});
        base.OnModelCreating(modelBuilder);
    }

    public AppDbContext([NotNullAttribute]DbContextOptions options ) : base(options)
    {
        
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
                entry.Entity.UpdatedAt = DateTime.Now;
            }

            if(entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<GenreEntity> Generes {  get; set; }
    public DbSet<ActorEntity> Actors { get; set; }
    public DbSet<MovieTheaterEntity> MovieTheaters { get; set; }
    public DbSet<MovieEntity> Movies { get; set; }
    public DbSet<MovieActorsEntity> MoviesActors { get; set; }
    public DbSet<MovieGenres> MovieGenres { get; set; }
    public DbSet<MovieTheaterMovieEntity> MovieTheaterMovies { get; set; }
    public DbSet<RatingEntity> Ratings { get; set; }
}
