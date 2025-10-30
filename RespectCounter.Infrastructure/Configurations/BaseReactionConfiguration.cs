using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public class BaseReactionConfiguration: AuditableConfiguration<BaseReaction>
{
    public override void Configure(EntityTypeBuilder<BaseReaction> builder)
    {
        base.Configure(builder);

        builder.UseTpcMappingStrategy(); 
    }
}