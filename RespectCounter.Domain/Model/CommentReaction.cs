using RespectCounter.Domain.Enums;

namespace RespectCounter.Domain.Model;

public class CommentReaction : BaseReaction
{
    public CommentReaction() : base() { }
    public CommentReaction(Comment comment, ReactionType type, User user, DateTime now) : base(type, user, now)
    {
        CommentId = comment.Id;
        Comment = comment;
    }

    public Guid CommentId { get; set; }
    public virtual Comment Comment { get; set; } = null!;
}