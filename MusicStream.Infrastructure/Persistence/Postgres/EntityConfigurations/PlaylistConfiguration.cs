using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Persistence.Postgres.EntityConfigurations;

internal class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasOne(p => p.Subscription)
        .WithMany(s => s.Playlists).HasForeignKey(p => p.SubscriptionId);
        builder.Property(p => p.MusicLimits).HasDefaultValue(10);
    }
}
