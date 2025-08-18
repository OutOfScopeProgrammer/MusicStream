using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Persistence.Postgres.EntityConfigurations;

public class MusicConfiguration : IEntityTypeConfiguration<Music>
{
    public void Configure(EntityTypeBuilder<Music> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Title).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Description).HasMaxLength(150);


    }
}
