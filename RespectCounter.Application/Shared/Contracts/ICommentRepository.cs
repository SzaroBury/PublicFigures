using RespectCounter.Application.Shared.Enums;
using RespectCounter.Domain.Enums;
using RespectCounter.Domain.Model;
using DomainComment = RespectCounter.Domain.Model.Comment;

namespace RespectCounter.Application.Shared.Contracts;

public interface ICommentRepository
{
    Task<PagedResult<DomainComment>> GetPagedCommentsForActivityAsync(Guid activityId, CommentSortBy sortBy, IReadOnlyCollection<CommentStatus> statuses, int page, int pageSize, CancellationToken cancellationToken);
    Task<PagedResult<DomainComment>> GetPagedCommentsForPersonAsync(Guid personId, CommentSortBy sortBy, IReadOnlyCollection<CommentStatus> statuses, int page, int pageSize, CancellationToken cancellationToken);
    Task<DomainComment> GetCommentByIdAsync(Guid guid, CancellationToken cancellationToken);
    void AddComment(DomainComment comment);
    Task UpdateAncestorsCountsDirectAsync(Guid parentGuid, CancellationToken cancellationToken);
}