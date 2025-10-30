using MediatR;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;
using DomainComment = RespectCounter.Domain.Model.Comment;

namespace RespectCounter.Application.Comment.Commands;

public record HideCommentCommand(string CommentId, string UserId): IRequest<CommentDTO>;

public class HideCommentCommandHandler : IRequestHandler<HideCommentCommand, CommentDTO>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;

    public HideCommentCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<CommentDTO> Handle(HideCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var commentGuid = request.CommentId.ToGuid();
        var comment = await _repository.FindByIdAsync<DomainComment>(commentGuid, cancellationToken)
            ?? throw new InvalidOperationException($"The Activity with ID {request.CommentId} was not found in the system, despite the previous validation check.");

        comment.Hide(user);
        _uow.GetWriteRepository().Update(comment);
        await _uow.CommitAsync(cancellationToken);

        return comment.ToDTO(1, userId);
    }
}