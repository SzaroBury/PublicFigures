using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Domain.Model;

public class Person : Entity, IReactionable<PersonReaction>
{
    public Person() : base() { }
    public Person(User user, DateTime? now) : base(user, now) { }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string NickName { get; set; } = string.Empty;
    public string Profession { get; set; } = "Unknown";
    public string Description { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateOnly? Birthday { get; set; }
    public DateOnly? DeathDate { get; set; }
    public PersonStatus Status { get; set; } = PersonStatus.NotVerified;
    public virtual ICollection<Activity> Activities { get; init; } = [];
    public virtual ICollection<Comment> Comments { get; init; } = [];
    public virtual ICollection<PersonReaction> Reactions { get; init; } = [];
    public virtual ICollection<PersonTag> Tags { get; init; } = [];

       public void Verify(User user, DateTime? now = null)
    {
        Updated(user, now);
        Status = PersonStatus.Verified;
    }

    public void Hide(User user, DateTime? now = null)
    {
        Updated(user, now);
        Status = PersonStatus.Hidden;
    }
}