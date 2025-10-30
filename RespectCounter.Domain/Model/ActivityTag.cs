namespace RespectCounter.Domain.Model;

public class ActivityTag : Auditable
{
    public ActivityTag(): base() { }
    public ActivityTag(Activity activity, Tag tag, User user, DateTime now) : base(user, now)
    {
        ActivityId = activity.Id;
        Activity = activity;
        TagId = tag.Id;
        Tag = tag;
    }

    public Guid ActivityId { get; set; }
    public virtual Activity Activity { get; set; } = null!;
    public Guid TagId { get; set; }
    public virtual Tag Tag { get; set; } = null!;
}