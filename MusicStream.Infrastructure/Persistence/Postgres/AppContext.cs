using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Persistence.Postgres;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Music> Musics { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Singer> Singers { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

}
