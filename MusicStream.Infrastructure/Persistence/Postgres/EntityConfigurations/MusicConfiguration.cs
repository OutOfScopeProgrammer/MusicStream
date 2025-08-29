using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Persistence.Postgres.EntityConfigurations;

internal class MusicConfiguration : IEntityTypeConfiguration<Domain.Entities.Music>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Music> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Title).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Artist).HasMaxLength(100);
        builder.Property(m => m.Date).HasMaxLength(15);
        builder.Property(m => m.Duration).HasMaxLength(20);
        builder.Property(m => m.Genre).HasMaxLength(50);

        builder.HasMany(m => m.Playlist)
        .WithMany(p => p.Musics);


    }
}
