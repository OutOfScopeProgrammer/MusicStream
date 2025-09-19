using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Persistence.Postgres.EntityConfigurations;

internal class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.PlaylistLimit).IsRequired();
        builder.Property(s => s.SubscriptionType).IsRequired()
        .HasConversion<string>();

    }

}
