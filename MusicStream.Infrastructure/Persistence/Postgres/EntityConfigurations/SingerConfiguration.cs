using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Persistence.Postgres.EntityConfigurations;

internal class SingerConfiguration : IEntityTypeConfiguration<Singer>
{
    public void Configure(EntityTypeBuilder<Singer> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.FirstName).HasMaxLength(50);
        builder.Property(s => s.LastName).HasMaxLength(50);
        builder.HasMany(s => s.Musics)
        .WithOne(m => m.Singer)
        .HasForeignKey(m => m.SingerId);

    }
}
