using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Persistence.Postgres.EntityConfigurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.Token).IsUnique();
        builder.HasOne(r => r.User)
        .WithOne(u => u.RefreshToken)
        .HasForeignKey<RefreshToken>(r => r.UserId);
    }
}
