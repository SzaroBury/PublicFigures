using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public class CommentConfiguration: AuditableConfiguration<Comment>
{
    public override void Configure(EntityTypeBuilder<Comment> builder)
    {
        base.Configure(builder);

        builder.HasOne(c => c.Activity)
           .WithMany(a => a.Comments)
           .HasForeignKey(c => c.ActivityId)
           .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.Person)
               .WithMany(p => p.Comments)
               .HasForeignKey(c => c.PersonId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.Parent)
               .WithMany(c => c.Children)
               .HasForeignKey(c => c.ParentId)
               .OnDelete(DeleteBehavior.NoAction);
                                                
        builder.HasMany(c => c.Reactions)
            .WithOne(cr => cr.Comment)
            .HasForeignKey(cr => cr.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}