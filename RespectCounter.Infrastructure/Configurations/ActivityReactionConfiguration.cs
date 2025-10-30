using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public class ActivityReactionConfiguration: AuditableConfiguration<ActivityReaction>
{
    public override void Configure(EntityTypeBuilder<ActivityReaction> builder)
    {
        base.Configure(builder);

        builder.ToTable("ActivityReactions");

        builder.HasOne(ar => ar.Activity)
           .WithMany(a => a.Reactions)
           .HasForeignKey(ar => ar.ActivityId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}