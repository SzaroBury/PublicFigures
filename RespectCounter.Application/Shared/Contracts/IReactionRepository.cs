using DomainActivityReaction = RespectCounter.Domain.Model.ActivityReaction;
using DomainCommentReaction = RespectCounter.Domain.Model.CommentReaction;
using DomainPersonReaction = RespectCounter.Domain.Model.PersonReaction;

namespace RespectCounter.Application.Shared.Contracts;

public interface IReactionRepository
{
    Task<DomainActivityReaction?> TryGetCurrentUserReactionForActivityAsync(Guid activityId, Guid userId, CancellationToken cancellationToken = default);
    Task<DomainCommentReaction?> TryGetCurrentUserReactionForCommentAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default);
    Task<DomainPersonReaction?> TryGetCurrentUserReactionForPersonAsync(Guid personId, Guid userId, CancellationToken cancellationToken = default);
}