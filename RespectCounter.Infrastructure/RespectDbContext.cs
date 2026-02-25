using RespectCounter.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using RespectCounter.Infrastructure.Configurations;

namespace RespectCounter.Infrastructure;

public class RespectDbContext : IdentityDbContext<CustomIdentityUser, IdentityRole<Guid>, Guid>
{
    //To add migration:         dotnet-ef migrations add <migration name> -p RespectCounter.Infrastructure -s RespectCounter.API -c RespectDbContext
    //To apply migration:       dotnet-ef update database

    public DbSet<Activity> Activities { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> DomainUsers { get; set; }

    public DbSet<ActivityReaction> ActivityReactions { get; set; }
    public DbSet<PersonReaction> PersonReactions { get; set; }
    public DbSet<CommentReaction> CommentReactions { get; set; }

    public DbSet<ActivityTag> ActivityTags { get; set; }
    public DbSet<PersonTag> PersonTag { get; set; }

    public RespectDbContext(DbContextOptions<RespectDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ActivityConfiguration());
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        modelBuilder.ApplyConfiguration(new BaseReactionConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityReactionConfiguration());
        modelBuilder.ApplyConfiguration(new PersonReactionConfiguration());
        modelBuilder.ApplyConfiguration(new CommentReactionConfiguration());

        modelBuilder.ApplyConfiguration(new PersonTagConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityTagConfiguration());

        SeedData.Seed(modelBuilder);
    }
}