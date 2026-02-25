using MediatR;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Application.Shared.Extensions;
using DomainComment = RespectCounter.Domain.Model.Comment;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared;

namespace RespectCounter.Application.Comment.Queries;

public record GetCommentsForPersonQuery(
    string PersonId,
    int Levels,
    int Page,
    int PageSize,
    string? Order = null,
    string? UserId = null
) : IRequest<PagedResult<CommentDTO>>;

public class GetCommentsForPersonQueryHandler : IRequestHandler<GetCommentsForPersonQuery, PagedResult<CommentDTO>>
{
    private readonly ICommentRepository _commentRepository;

    public GetCommentsForPersonQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<PagedResult<CommentDTO>> Handle(GetCommentsForPersonQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = request.UserId.ToNullableGuid();
        var personId = request.PersonId.ToGuid();
        var order = CommentSortBy.LatestAdded;
        order = request.Order.ToCommentSortByEnum();
        IEnumerable<CommentStatus> statuses = [CommentStatus.Created, CommentStatus.Edited];

        var pagedComments = await _commentRepository.GetPagedCommentsForPersonAsync(personId, order, statuses, request.Page, request.PageSize, cancellationToken);

        return new PagedResult<CommentDTO>
        {
            Items = pagedComments.Items.Select(c => c.ToDTO(request.Levels, userId)),
            TotalItems = pagedComments.TotalItems,
            PageNumber = pagedComments.PageNumber,
            PageSize = pagedComments.PageSize
        };
    }
}