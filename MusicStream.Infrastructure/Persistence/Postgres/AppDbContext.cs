using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Persistence.Postgres;

internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Music> Musics { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Singer> Singers { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

}
