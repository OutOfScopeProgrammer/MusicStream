using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Persistence.Postgres.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.HasKey(u => u.Id);
        builder.Property(u => u.PhoneNumber).HasMaxLength(11).IsRequired();
        builder.Property(u => u.HashedPassword).IsRequired();
        builder.HasOne(u => u.Subscription)
        .WithOne(s => s.User)
        .HasForeignKey<Subscription>(s => s.UserId);


        builder.HasIndex(u => u.PhoneNumber).IsUnique();
    }
}
