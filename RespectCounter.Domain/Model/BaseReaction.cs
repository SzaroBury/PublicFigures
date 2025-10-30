using RespectCounter.Domain.Enums;

namespace RespectCounter.Domain.Model;

public abstract class BaseReaction : Entity
{
    public BaseReaction(): base() {}
    public BaseReaction(ReactionType type, User user, DateTime? now) : base(user, now)
    {
        ReactionType = type;
    }

    public ReactionType ReactionType { get; set; } = ReactionType.Like;
}