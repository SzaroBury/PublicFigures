using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public class PersonTagConfiguration: AuditableConfiguration<PersonTag>
{
    public override void Configure(EntityTypeBuilder<PersonTag> builder)
    {
        base.Configure(builder);

        builder.HasKey(pt => new { pt.PersonId, pt.TagId });
    }
}