using RespectCounter.Domain.Enums;

namespace RespectCounter.Domain.Model;

public class ActivityReaction : BaseReaction
{
    public ActivityReaction() : base() { }
    public ActivityReaction(Activity activity, ReactionType type, User user, DateTime now) : base(type, user, now)
    {
        ActivityId = activity.Id;
        Activity = activity;
    }

    public Guid ActivityId { get; set; }
    public virtual Activity Activity { get; set; } = null!;
}