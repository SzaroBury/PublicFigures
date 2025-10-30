using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Identity;

namespace RespectCounter.Infrastructure.Configurations;

public class UserConfiguration: AuditableConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasOne<CustomIdentityUser>()
            .WithOne(ciu => ciu.Profile)
            .HasForeignKey<User>(u => u.Id)
            .IsRequired();
        
        builder.HasMany(u => u.RecentlyBrowsedTags)
            .WithMany()
            .UsingEntity(j => j.ToTable("UserRecentlyBrowsedTags")); 

        builder.HasMany(u => u.FavoriteTags)
            .WithMany()
            .UsingEntity(j => j.ToTable("UserFavoriteTags"));
        
        builder.HasIndex(u => u.Username)
               .IsUnique();
        builder.Property(u => u.Username)
               .HasMaxLength(256);
    }
}