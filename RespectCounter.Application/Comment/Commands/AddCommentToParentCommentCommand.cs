using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainComment = RespectCounter.Domain.Model.Comment;

namespace RespectCounter.Application.Comment.Commands
{
    public record AddCommentToParentCommentCommand(
        string ParentCommentId,
        string Content,
        string UserId
    ) : IRequest<CommentDTO>;

    public class AddCommentToParentCommentCommandHandler : IRequestHandler<AddCommentToParentCommentCommand, CommentDTO>
    {
        private readonly IReadOnlyRepository _repository;
        private readonly IUnitOfWork _uow;

        public AddCommentToParentCommentCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<CommentDTO> Handle(AddCommentToParentCommentCommand request, CancellationToken cancellationToken)
        {
            await _uow.BeginTransactionAsync(cancellationToken);
            try
            {
                var userId = request.UserId.ToGuid();
                var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
                    ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

                var parentCommentId = request.ParentCommentId.ToGuid();
                var parentComment = await _repository.SingleOrDefaultAsync<DomainComment>(
                    comment => comment.Id == parentCommentId,
                    "Parent",
                    cancellationToken
                ) ?? throw new KeyNotFoundException("There is no comment with the given id value.");

                parentComment.DirectChildrenCount++;
                parentComment.AllChildrenCount++;

                DateTime now = DateTime.UtcNow;
                DomainComment comment = new(user, now)
                {
                    ParentId = parentComment.Id,
                    Content = request.Content,
                };

                parentComment.Children.Add(comment);
                _uow.GetWriteRepository().Update(parentComment);
                _uow.GetWriteRepository().Add(comment);

                DomainComment? nextParentComment = parentComment.Parent;
                while (nextParentComment != null)
                {
                    nextParentComment.AllChildrenCount++;
                    _uow.GetWriteRepository().Update(nextParentComment);
                    nextParentComment = await _repository.SingleOrDefaultAsync<DomainComment>(
                        comment => comment.Id == parentComment.ParentId,
                        cancellationToken: cancellationToken
                    );
                }

                await _uow.CommitTransactionAsync(cancellationToken);
                return parentComment.ToDTO(1, user.Id);
            }
            catch
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}