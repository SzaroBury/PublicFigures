using RespectCounter.Application.Shared.Enums;
using RespectCounter.Domain.Enums;
using RespectCounter.Domain.Model;
using DomainComment = RespectCounter.Domain.Model.Comment;

namespace RespectCounter.Application.Shared.Contracts;

public interface ICommentRepository
{
    Task<PagedResult<DomainComment>> GetPagedCommentsForActivityAsync(Guid activityId, CommentSortBy sortBy, IEnumerable<CommentStatus> statuses, int page, int pageSize, CancellationToken cancellationToken);
    Task<PagedResult<DomainComment>> GetPagedCommentsForPersonAsync(Guid personId, CommentSortBy sortBy, IEnumerable<CommentStatus> statuses, int page, int pageSize, CancellationToken cancellationToken);
    Task<DomainComment> GetCommentByIdAsync(Guid guid, CancellationToken cancellationToken);
    Task UpdateAncestorsCountsAsync(Guid parentGuid, CancellationToken cancellationToken);
}