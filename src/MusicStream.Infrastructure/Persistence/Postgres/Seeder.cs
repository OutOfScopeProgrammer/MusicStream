using Microsoft.EntityFrameworkCore;
using MusicStream.Application.Interfaces;

namespace MusicStream.Infrastructure.Persistence.Postgres;

internal class Seeder(AppDbContext dbContext) : ISeeder
{
    public async Task MigrateAsync()
    {
        Console.WriteLine("Applying migrations...");
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("End");
    }
}
