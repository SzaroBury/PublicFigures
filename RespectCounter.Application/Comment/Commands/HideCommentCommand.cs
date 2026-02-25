using MediatR;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Comment.Commands;

public record HideCommentCommand(string CommentId, string UserId): IRequest<CommentDTO>;

public class HideCommentCommandHandler : IRequestHandler<HideCommentCommand, CommentDTO>
{
    private readonly IReadOnlyRepository _readOnlyRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _uow;

    public HideCommentCommandHandler(IReadOnlyRepository readOnlyRepository, ICommentRepository commentRepository, IUnitOfWork uow)
    {
        _readOnlyRepository = readOnlyRepository;
        _commentRepository = commentRepository;
        _uow = uow;
    }

    public async Task<CommentDTO> Handle(HideCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _readOnlyRepository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var commentGuid = request.CommentId.ToGuid();
        var comment = await _commentRepository.GetCommentByIdAsync(commentGuid, cancellationToken)
            ?? throw new InvalidOperationException($"The Comment with ID {request.CommentId} was not found in the system, despite the previous validation check.");

        comment.Hide(user);
        await _uow.CommitAsync(cancellationToken);

        return comment.ToDTO(1, userId);
    }
}