using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public class ActivityTagConfiguration: AuditableConfiguration<ActivityTag>
{
    public override void Configure(EntityTypeBuilder<ActivityTag> builder)
    {
        base.Configure(builder);

        builder.HasKey(at => new { at.ActivityId, at.TagId });
        
        // builder.HasOne(at => at.Activity)
        //     .WithMany(a => a.Tags)
        //     .HasForeignKey(at => at.ActivityId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // builder.HasOne(at => at.Tag)
        //     .WithMany(t => t.Activities)
        //     .HasForeignKey(at => at.TagId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}