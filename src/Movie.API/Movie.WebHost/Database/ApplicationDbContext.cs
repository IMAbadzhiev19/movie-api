using Microsoft.EntityFrameworkCore;
using Movie.WebHost.Entities;

namespace Movie.WebHost.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    
    
    public DbSet<Entities.Actor> Actors { get; set; } = null!;
    public DbSet<Entities.Movie> Movies { get; set; } = null!;
}