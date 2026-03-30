using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.Contracts;

using DomainComment = RespectCounter.Domain.Model.Comment;

namespace RespectCounter.Application.Comment.Commands;

public record AddCommentToParentCommentCommand(
    string ParentCommentId,
    string Content,
    string UserId
) : IRequest<CommentDTO>;

public class AddCommentToParentCommentCommandHandler : IRequestHandler<AddCommentToParentCommentCommand, CommentDTO>
{
    private readonly IReadOnlyRepository _readOnlyRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _uow;

    public AddCommentToParentCommentCommandHandler(IReadOnlyRepository readOnlyRepository, ICommentRepository commentRepository, IUnitOfWork uow)
    {
        _readOnlyRepository = readOnlyRepository;
        _commentRepository = commentRepository;
        _uow = uow;
    }

    public async Task<CommentDTO> Handle(AddCommentToParentCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _uow.BeginTransactionAsync(cancellationToken);
            
            var userId = request.UserId.ToGuid();
            var user = await _readOnlyRepository.FindByIdAsync<User>(userId, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

            var parentCommentId = request.ParentCommentId.ToGuid();
            var parentComment = await _commentRepository.GetCommentByIdAsync(parentCommentId, cancellationToken)
                ?? throw new KeyNotFoundException("There is no comment with the given id value.");

            DateTime now = DateTime.UtcNow;
            DomainComment comment = new(user.Id)
            {
                ParentId = parentComment.Id,
                Content = request.Content,
            };
            _commentRepository.AddComment(comment);
            await _uow.CommitAsync(cancellationToken);

            await _commentRepository.UpdateAncestorsCountsDirectAsync(parentCommentId, cancellationToken);
            await _uow.CommitTransactionAsync(cancellationToken);
            
            return comment.ToDTO(0, user.Id);    
        }
        catch (Exception)
        {
            await _uow.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}