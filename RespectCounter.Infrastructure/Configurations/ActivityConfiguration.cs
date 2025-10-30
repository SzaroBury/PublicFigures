using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public class ActivityConfiguration: AuditableConfiguration<Activity>
{
    public override void Configure(EntityTypeBuilder<Activity> builder)
    {
        base.Configure(builder);

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.Type)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasMany(a => a.Tags)
            .WithOne(at => at.Activity)
            .HasForeignKey(at => at.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Comments)
            .WithOne(c => c.Activity)
            .HasForeignKey(c => c.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Reactions)
            .WithOne(r => r.Activity)
            .HasForeignKey(r => r.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(a => a.Person)
            .WithMany(p => p.Activities)
            .HasForeignKey(a => a.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}