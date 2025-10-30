using MediatR;
using RespectCounter.Domain.Enums;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainComment = RespectCounter.Domain.Model.Comment;

namespace RespectCounter.Application.Comment.Queries;

public record GetCommentsForActivityQuery(
    string ActivityId,
    int Levels,
    int Page,
    int PageSize,
    string? Order = null,
    string? UserId = null
) : IRequest<IEnumerable<CommentDTO>>;

public class GetCommentsForActivityQueryHandler : IRequestHandler<GetCommentsForActivityQuery, IEnumerable<CommentDTO>>
{
    private readonly IReadOnlyRepository _repository;

    public GetCommentsForActivityQueryHandler(IReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CommentDTO>> Handle(GetCommentsForActivityQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = request.UserId.ToNullableGuid();

        var activityId = request.ActivityId.ToGuid();
        var query = _repository.FindQueryable<DomainComment>(
            c => c.ActivityId == activityId && c.Status != CommentStatus.Hidden
        );

        var order = CommentSortBy.LatestAdded;
        if (!string.IsNullOrWhiteSpace(request.Order))
        {
            order = request.Order.ToCommentSortByEnum();
        }
        var orderedQuery = query.ApplySorting(order);
        orderedQuery = orderedQuery.ApplyPaging(request.Page, request.PageSize);

        var comments = await _repository.FindListAsync(
            orderedQuery,
            ["Children", "Reactions", "CreatedBy", "LastUpdatedBy"],
            q => q.OrderByDescending(c => c.Created),
            cancellationToken
        );

        return comments.Select(c => c.ToDTO(request.Levels, userId));
    }
}