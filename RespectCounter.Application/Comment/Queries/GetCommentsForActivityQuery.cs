using MediatR;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Shared;

namespace RespectCounter.Application.Comment.Queries;

public record GetCommentsForActivityQuery(
    string ActivityId,
    int Levels,
    int Page,
    int PageSize,
    string? Order = null,
    string? UserId = null
) : IRequest<PagedResult<CommentDTO>>;

public class GetCommentsForActivityQueryHandler : IRequestHandler<GetCommentsForActivityQuery, PagedResult<CommentDTO>>
{
    private readonly ICommentRepository _commentRepository;

    public GetCommentsForActivityQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<PagedResult<CommentDTO>> Handle(GetCommentsForActivityQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToNullableGuid();
        var activityId = request.ActivityId.ToGuid();
        var order = request.Order.ToCommentSortByEnum();
        IEnumerable<CommentStatus> statuses = [CommentStatus.Created, CommentStatus.Edited];

        var comments = await _commentRepository.GetPagedCommentsForActivityAsync(activityId, order, statuses, request.Page, request.PageSize, cancellationToken);

        return new PagedResult<CommentDTO>
        {
            Items = comments.Items.Select(c => c.ToDTO(request.Levels, userId)),
            TotalItems = comments.TotalItems,
            PageNumber = comments.PageNumber,
            PageSize = comments.PageSize,
        };
    }
}