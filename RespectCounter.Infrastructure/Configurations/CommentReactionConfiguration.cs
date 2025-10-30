using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public class CommentReactionConfiguration: AuditableConfiguration<CommentReaction>
{
    public override void Configure(EntityTypeBuilder<CommentReaction> builder)
    {
        base.Configure(builder);

        builder.ToTable("CommentReactions");

        builder.HasOne(cr => cr.Comment)
               .WithMany(c => c.Reactions)
               .HasForeignKey(cr => cr.CommentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}