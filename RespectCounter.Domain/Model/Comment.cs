using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Domain.Model;

public class Comment : Entity, IReactionable<CommentReaction>
{
    public Comment() : base() { }
    public Comment(User user) : base(user, DateTime.UtcNow) { }
    public Comment(User user, DateTime now) : base(user, now) { }

    public string Content { get; set; } = string.Empty;
    public CommentStatus Status { get; set; } = CommentStatus.Created;
    public int DirectChildrenCount { get; set; } = 0;
    public int AllChildrenCount { get; set; } = 0;

    public Guid? ActivityId { get; set; }
    public virtual Activity? Activity { get; set; }
    public Guid? PersonId { get; set; }
    public virtual Person? Person { get; set; }
    public Guid? ParentId { get; set; }
    public virtual Comment? Parent { get; set; }
    public virtual ICollection<CommentReaction> Reactions { get; init; } = [];
    public virtual ICollection<Comment> Children { get; set; } = [];
    public int ChildrenCount => Children.Count + Children.Sum(c => c.ChildrenCount);

    public void Hide(User user, DateTime? now = null)
    {
        Updated(user, now);
        Status = CommentStatus.Hidden;
    }

    public void Edit(string content, User user, DateTime? now = null)
    {
        Updated(user, now);
        Content = content;
    }
}