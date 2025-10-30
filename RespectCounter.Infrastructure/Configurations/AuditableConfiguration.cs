using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public abstract class AuditableConfiguration<T>() : IEntityTypeConfiguration<T> where T : Auditable
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasOne(a => a.CreatedBy)
            .WithMany()
            .HasForeignKey(a => a.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(a => a.LastUpdatedBy)
            .WithMany()
            .HasForeignKey(a => a.LastUpdatedById)
            .OnDelete(DeleteBehavior.NoAction);
    }
}