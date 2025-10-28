using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserDto> Users { get; set; }
    public DbSet<AiModelDto> Models { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<UserDto>();
        user.HasKey(u => u.Id);

        var model = modelBuilder.Entity<AiModelDto>();
        model.HasKey(m => m.Id);
    }
}