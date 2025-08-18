using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Persistence.Postgres.EntityConfigurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.PlaylistLimit).IsRequired();

    }

}
