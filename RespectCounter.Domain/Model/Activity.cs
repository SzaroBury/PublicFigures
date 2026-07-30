using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Domain.Model;

public class Activity : Entity, IReactionable<ActivityReaction>
{
    public Activity() : base() { }
    public Activity(User user, DateTime? now) : base(user, now) { }

    public string Value { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime? Happend { get; set; }
    public string Source { get; set; } = string.Empty;
    public ActivityStatus Status { get; set; } = ActivityStatus.NotVerified;
    public ActivityType Type { get; set; } = ActivityType.Action;
    public Guid PersonId { get; set; }
    public virtual Person Person { get; set; } = null!;
    public virtual ICollection<ActivityReaction> Reactions { get; init; }  = [];
    public virtual ICollection<Comment> Comments { get; init; } = [];
    public virtual ICollection<ActivityTag> Tags { get; init; } = [];

    public void Verify(User user, DateTime? now = null)
    {
        Updated(user, now);
        Status = ActivityStatus.Verified;
    }
    
    public void Hide(User user, DateTime? now = null)
    {
        Updated(user, now);
        Status = ActivityStatus.Hidden;
    }
}