using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Comment.Commands;

public record UpdateCommentCommand(
    string CommentId,
    string Content,
    string UserId
) : IRequest<CommentDTO>;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentDTO>
{
    private readonly IReadOnlyRepository _readOnlyRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _uow;

    public UpdateCommentCommandHandler(IReadOnlyRepository repository, ICommentRepository commentRepository, IIdentityService identityService, IUnitOfWork uow)
    {
        _readOnlyRepository = repository;
        _commentRepository = commentRepository;
        _identityService = identityService;
        _uow = uow;
    }

    public async Task<CommentDTO> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _readOnlyRepository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var commentGuid = request.CommentId.ToGuid();
        var comment = await _commentRepository.GetCommentByIdAsync(commentGuid, cancellationToken)
            ?? throw new KeyNotFoundException($"The Comment with ID {request.CommentId} was not found in the system, despite the previous validation check.");

        var isOwnerOfTheComment = userId == comment.CreatedById;
        var isAdmin = await _identityService.IsInRoleAsync(userId, "Admin", cancellationToken);

        if (!isOwnerOfTheComment && !isAdmin)
        {
            throw new UnauthorizedAccessException($"User {userId} is not authorized to update comment {commentGuid}. Access is denied.");
        }

        DateTime now = DateTime.UtcNow;
        comment.Edit(request.Content, user, now);

        await _uow.CommitAsync(cancellationToken);
        return comment.ToDTO(1, userId);
    }
}