using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public class PersonReactionConfiguration: AuditableConfiguration<PersonReaction>
{
    public override void Configure(EntityTypeBuilder<PersonReaction> builder)
    {
        base.Configure(builder);

        builder.ToTable("PersonReactions");

        builder.HasOne(pr => pr.Person)
               .WithMany(p => p.Reactions)
               .HasForeignKey(pr => pr.PersonId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}