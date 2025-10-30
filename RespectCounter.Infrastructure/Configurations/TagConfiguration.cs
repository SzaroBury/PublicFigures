using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public class TagConfiguration: AuditableConfiguration<Tag>
{
    public override void Configure(EntityTypeBuilder<Tag> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(100);

       builder.HasIndex(t => t.Name)
              .IsUnique();
               
       builder.HasMany(t => t.Activities)
              .WithOne(at => at.Tag)
              .HasForeignKey(at => at.TagId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}