using RespectCounter.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Infrastructure.Identity;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Infrastructure;

public static class SeedData
{
    public static readonly Guid SystemUserId = new("00000000-0000-0000-0000-000000000001");

    public static void Seed(ModelBuilder mb)
    {
        DateTime now = DateTime.UtcNow;

        SeedIdentity(mb);

        Guid RL = Guid.NewGuid();
        Guid RK = Guid.NewGuid();
        Guid AD = Guid.NewGuid();
        Guid DT = Guid.NewGuid();
        mb.Entity<Person>().HasData(new List<Person>
        {
            CreateDummyPerson(RL, "Robert", "Lewandowski", "Footballer", new DateOnly(1988, 08, 21), "Polish", nickname: "Lewy"),
            CreateDummyPerson(RK, "Robert", "Kubica", "Racer", new DateOnly(1984, 12, 07), "Polish", status: PersonStatus.NotVerified),
            CreateDummyPerson(AD, "Andrzej", "Duda", "Politician", new DateOnly(1972, 05, 16), "Polish"),
            CreateDummyPerson(DT, "Donald", "Tusk", "Politician", new DateOnly(1957, 04, 22), "Polish")
        });

        Guid RLactivity = Guid.NewGuid();
        Guid RKactivity = Guid.NewGuid();
        mb.Entity<Activity>().HasData(new List<Activity>
        {
            CreateDummyActivity(RLactivity, RL, "Milik jest słaby", "", "Test description", "Dude, just trust me", type: ActivityType.Quote),
            CreateDummyActivity(RKactivity, RK, "Monaco GP 2010: Robeeeeeeeert Kubica P2 in Quali", "Monaco, MC", "Można utknąć w eeeee korku", "https://www.youtube.com/watch?v=qbYMoKxif6I", type:ActivityType.Act, happend: new DateTime(2010, 05, 15), status: ActivityStatus.Verified)
        });

        Guid RLComment = Guid.NewGuid();
        Guid RLActivityComment = Guid.NewGuid();
        Guid RLActivityNegativeComment = Guid.NewGuid();
        Guid ADComment = Guid.NewGuid();
        Guid ADResponse = Guid.NewGuid();

        mb.Entity<Comment>().HasData(
        [
            CreateDummyComment(RLComment, "Najlepszy zawodnik!",                    2, 2, perId: RL),
            CreateDummyComment(Guid.NewGuid(), "No nie wiem. Milik lepszy!",        parentId: RLComment),
            CreateDummyComment(Guid.NewGuid(), "Jest całkiem dobry faktycznie",     parentId: RLComment),
            CreateDummyComment(RLActivityComment, "Fajność!",                       2, 2, actId: RLactivity),
            CreateDummyComment(Guid.NewGuid(), "Zgadza się!",                       parentId: RLActivityComment),
            CreateDummyComment(Guid.NewGuid(), "Też się zgadzam. Fajność!",         parentId: RLActivityComment),
            CreateDummyComment(RLActivityNegativeComment, "Niefajność",             1, 1, actId: RLactivity),
            CreateDummyComment(Guid.NewGuid(), "Nie zgadzam się. Fajność.",         parentId: RLActivityNegativeComment),
            CreateDummyComment(Guid.NewGuid(), "Lepsza weeeeeersja: https://www.youtube.com/watch?v=vmLonweq6wA", actId: RKactivity),
            CreateDummyComment(ADComment, "Bardzo memiczna osoba",                  2, 3, perId: AD),
            CreateDummyComment(Guid.NewGuid(), "Hańba!",                            parentId : ADComment),
            CreateDummyComment(ADResponse, "Chyba ty",                              parentId : ADComment),
            CreateDummyComment(Guid.NewGuid(), "Nie, bo ty",                        parentId : ADResponse),
            CreateDummyComment(Guid.NewGuid(), "Ja tam mu nei ufam",                perId: AD),
            CreateDummyComment(Guid.NewGuid(), "Nie lubiem go, bo Andrzej to dziwne imię", perId: AD)
        ]);

        Guid sportTag = Guid.NewGuid();
        Guid footballTag = Guid.NewGuid();
        Guid fcbarcelonaTag = Guid.NewGuid();
        Guid f1Tag = Guid.NewGuid();
        Guid wecTag = Guid.NewGuid();
        Guid politicsTag = Guid.NewGuid();
        Guid pisTag = Guid.NewGuid();
        Guid poTag = Guid.NewGuid();
        mb.Entity<Tag>().HasData(new List<Tag>
        {
            CreateDummyTag(sportTag, "Sport"),
            CreateDummyTag(footballTag, "Football"),
            CreateDummyTag(fcbarcelonaTag, "FC Barcelona"),
            CreateDummyTag(f1Tag, "F1"),
            CreateDummyTag(wecTag, "WEC"),
            CreateDummyTag(politicsTag, "Politics"),
            CreateDummyTag(pisTag, "PiS"),
            CreateDummyTag(poTag, "PO"),
        });
        mb.Entity<PersonTag>().HasData(
            new PersonTag(RL, sportTag, SystemUserId, now),
            new PersonTag(RK, sportTag, SystemUserId, now),
            new PersonTag(RL, footballTag, SystemUserId, now),
            new PersonTag(RL, fcbarcelonaTag, SystemUserId, now),
            new PersonTag(RK, f1Tag, SystemUserId, now),
            new PersonTag(RK, wecTag, SystemUserId, now),
            new PersonTag(RK, politicsTag, SystemUserId, now),
            new PersonTag(DT, politicsTag, SystemUserId, now),
            new PersonTag(AD, pisTag, SystemUserId, now),
            new PersonTag(DT, poTag, SystemUserId, now)
        );
        mb.Entity<ActivityTag>().HasData(
            new ActivityTag(RLactivity, sportTag, SystemUserId, now),
            new ActivityTag(RLactivity, footballTag,  SystemUserId, now),
            new ActivityTag(RKactivity, sportTag, SystemUserId, now),
            new ActivityTag(RKactivity, f1Tag, SystemUserId, now)
        );
        mb.Entity<PersonReaction>().HasData(new List<PersonReaction>
        {
            CreateDummyPersonReaction(Guid.NewGuid(), ReactionType.Hate,    perId: RL),
            CreateDummyPersonReaction(Guid.NewGuid(), ReactionType.Like,    perId: RL),
            CreateDummyPersonReaction(Guid.NewGuid(), ReactionType.Dislike, perId: RL),
            CreateDummyPersonReaction(Guid.NewGuid(), ReactionType.Like,    perId: RL),
            CreateDummyPersonReaction(Guid.NewGuid(), ReactionType.Love,    perId: RL),
            CreateDummyPersonReaction(Guid.NewGuid(), ReactionType.Love,    perId: RL),
            CreateDummyPersonReaction(Guid.NewGuid(), ReactionType.Love,    perId: RL),
            CreateDummyPersonReaction(Guid.NewGuid(), ReactionType.Love,    perId: RL),
        });
        mb.Entity<ActivityReaction>().HasData(new List<ActivityReaction>
        {
            CreateDummyActivityReaction(Guid.NewGuid(), ReactionType.Like,    actId: RLactivity),
            CreateDummyActivityReaction(Guid.NewGuid(), ReactionType.Dislike, actId: RLactivity),
            CreateDummyActivityReaction(Guid.NewGuid(), ReactionType.Like,    actId: RLactivity),
            CreateDummyActivityReaction(Guid.NewGuid(), ReactionType.Love,    actId: RLactivity),
        });
        mb.Entity<CommentReaction>().HasData(new List<CommentReaction>
        {
            CreateDummyCommentReaction(Guid.NewGuid(), ReactionType.Love, comId: RLComment),
            CreateDummyCommentReaction(Guid.NewGuid(), ReactionType.Like, comId: RLActivityComment),
            CreateDummyCommentReaction(Guid.NewGuid(), ReactionType.Love, comId: RLActivityComment)
        });
    }

    private static void SeedIdentity(ModelBuilder mb)
    {
        Guid adminRoleId = Guid.NewGuid();
        Guid userRoleId = Guid.NewGuid();
        Guid adminId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();

        mb.Entity<IdentityRole<Guid>>().HasData(new List<IdentityRole<Guid>>
        {
            new("Admin") { Id = adminRoleId },
            new("User") { Id = userRoleId }
        });

        var hasher = new PasswordHasher<CustomIdentityUser>();
        var now = DateTime.UtcNow;

        var systemIdentity = new CustomIdentityUser
        {
            Id = SystemUserId,
            UserName = "system_user",
            NormalizedUserName = "SYSTEM_USER",
        };
        var systemUser = new User
        {
            Id = SystemUserId,
            Username = "System",
            CreatedById = SystemUserId,
            LastUpdatedById = SystemUserId,
            Created = now,
            LastUpdated = now
        };
        mb.Entity<CustomIdentityUser>().HasData(systemIdentity);
        mb.Entity<User>().HasData(systemUser);

        var admin = new User
        {
            Id = adminId,
            Username = "admin",
            CreatedById = SystemUserId,
            LastUpdatedById = SystemUserId,
            Created = now,
            LastUpdated = now
        };
        var adminIdentity = new CustomIdentityUser
        {
            Id = adminId,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true
        };
        adminIdentity.PasswordHash = hasher.HashPassword(adminIdentity, "Admin123!");
        mb.Entity<CustomIdentityUser>().HasData(adminIdentity);
        mb.Entity<User>().HasData(admin);

        var user = new User
        {
            Id = userId,
            Username = "user",
            CreatedById = SystemUserId,
            LastUpdatedById = SystemUserId,
            Created = now,
            LastUpdated = now
        };
        var userIdentity = new CustomIdentityUser
        {
            Id = userId,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            UserName = "user",
            NormalizedUserName = "USER",
            Email = "user@example.com",
            NormalizedEmail = "USER@EXAMPLE.COM",
            EmailConfirmed = true
        };
        userIdentity.PasswordHash = hasher.HashPassword(userIdentity, "User123!");
        mb.Entity<CustomIdentityUser>().HasData(userIdentity);
        mb.Entity<User>().HasData(user);

        mb.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>()
            {
                UserId = adminId,
                RoleId = adminRoleId
            }
        );

        mb.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>()
            {
                UserId = userId,
                RoleId = userRoleId
            }
        );
    }

    private static Person CreateDummyPerson(
        Guid id,
        string firstName,
        string lastName,
        string profession,
        DateOnly birthDate,
        string nationality,
        string nickname = "",
        DateOnly? deathDate = null,
        PersonStatus status = PersonStatus.Verified,
        string desc = "Test desc")
    {
        var now = DateTime.UtcNow;
        var systemUserId = SystemUserId;
        return new Person
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            NickName = nickname,
            Profession = profession,
            Description = desc,
            Birthday = birthDate,
            DeathDate = deathDate.GetValueOrDefault(),
            Nationality = nationality,
            Status = status,
            AvatarUrl = null,

            Created = now,
            CreatedById = systemUserId,
            LastUpdated = now,
            LastUpdatedById = systemUserId
        };
    }
    
    private static Tag CreateDummyTag(
        Guid id,
        string name,
        string desc = "Test desc")
    {
        var now = DateTime.UtcNow;
        var systemUserId = SystemUserId;
        return new Tag
        {
            Id = id,
            Name = name,
            Description = desc,

            Created = now,
            CreatedById = systemUserId,
            LastUpdated = now,
            LastUpdatedById = systemUserId
        };
    }

    private static Activity CreateDummyActivity(
        Guid id,
        Guid personId,
        string val,
        string loc,
        string desc,
        string source,
        DateTime? happend = null,
        ActivityType type = ActivityType.Quote,
        ActivityStatus status = ActivityStatus.NotVerified)
    {
        var now = DateTime.UtcNow;
        var systemUserId = SystemUserId;

        return new Activity
        {
            Id = id,
            Location = loc,
            Value = val,
            Description = desc,
            Happend = happend.GetValueOrDefault(),
            Type = type,
            Source = source,
            Status = status,
            PersonId = personId,

            Created = now,
            CreatedById = systemUserId,
            LastUpdated = now,
            LastUpdatedById = systemUserId
        };
    }

    private static Comment CreateDummyComment(
        Guid id,
        string content,
        int directChildrenCount = 0,
        int allChildrenCount = 0,
        Guid? perId = null,
        Guid? actId = null,
        Guid? parentId = null)
    {
        var now = DateTime.UtcNow;
        var systemUserId = SystemUserId;

        return new Comment
        {
            Id = id,
            Content = content,
            DirectChildrenCount = directChildrenCount,
            AllChildrenCount = allChildrenCount,

            PersonId = perId,
            ParentId = parentId,
            ActivityId = actId,

            Created = now,
            CreatedById = systemUserId,
            LastUpdated = now,
            LastUpdatedById = systemUserId
        };
    }
    private static PersonReaction CreateDummyPersonReaction(
        Guid id,
        ReactionType type,
        Guid perId)
    {
        var now = DateTime.UtcNow;
        var systemUserId = SystemUserId;

        return new PersonReaction
        {
            Id = id,
            ReactionType = type,
            PersonId = perId,
            
            Created = now,
            CreatedById = systemUserId,
            LastUpdated = now,
            LastUpdatedById = systemUserId,
        };
    }

    private static ActivityReaction CreateDummyActivityReaction(
        Guid id,
        ReactionType type,
        Guid actId)
    {
        var now = DateTime.UtcNow;
        var systemUserId = SystemUserId;


        return new ActivityReaction
        {
            Id = id,
            ReactionType = type,
            ActivityId = actId,

            Created = now,
            CreatedById = systemUserId,
            LastUpdated = now,
            LastUpdatedById = systemUserId,
        };
    } 

    private static CommentReaction CreateDummyCommentReaction(
        Guid id,
        ReactionType type,
        Guid comId)
    {
        var now = DateTime.UtcNow;
        var systemUserId = SystemUserId;

        return new CommentReaction
        {
            Id = id,
            ReactionType = type,
            CommentId = comId,

            Created = now,
            CreatedById = systemUserId,
            LastUpdated = now,
            LastUpdatedById = systemUserId,
        };
    }     
}
