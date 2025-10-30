using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Configurations;

public class PersonConfiguration: AuditableConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.HasMany(p => p.Activities)
               .WithOne(a => a.Person)
               .HasForeignKey(a => a.PersonId)
               .OnDelete(DeleteBehavior.Restrict); 

        builder.HasMany(p => p.Comments)
               .WithOne(c => c.Person)
               .HasForeignKey(c => c.PersonId)
               .OnDelete(DeleteBehavior.SetNull);
               
        builder.HasMany(p => p.Reactions)
               .WithOne(pr => pr.Person)
               .HasForeignKey(pr => pr.PersonId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}