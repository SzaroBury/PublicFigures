using MediatR;
using RespectCounter.Domain.Enums;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Application.Shared.Extensions;
using DomainComment = RespectCounter.Domain.Model.Comment;

namespace RespectCounter.Application.Comment.Queries;

public record GetCommentsForPersonQuery(
    string PersonId,
    int Levels,
    int Page,
    int PageSize,
    string? Order = null,
    string? UserId = null
) : IRequest<IEnumerable<CommentDTO>>;

public class GetCommentsForPersonQueryHandler : IRequestHandler<GetCommentsForPersonQuery, IEnumerable<CommentDTO>>
{
    private readonly IReadOnlyRepository _repository;

    public GetCommentsForPersonQueryHandler(IReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CommentDTO>> Handle(GetCommentsForPersonQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = request.UserId.ToNullableGuid();

        var personId = request.PersonId.ToGuid();
        var query = _repository.FindQueryable<DomainComment>(
            c => c.PersonId == personId && c.Status != CommentStatus.Hidden
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

        foreach (var comment in comments)
        {
            comment.Children = await _repository.FindListAsync<DomainComment>(
                c => c.ParentId == comment.Id && c.Status != CommentStatus.Hidden,
                null,
                q => q.OrderByDescending(c => c.Created),
                cancellationToken
            );
        }

        return comments.Select(c => c.ToDTO(request.Levels, userId));
    }
}