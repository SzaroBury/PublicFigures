using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainPerson = RespectCounter.Domain.Model.Person;
using DomainComment = RespectCounter.Domain.Model.Comment;

namespace RespectCounter.Application.Comment.Commands;

public record AddCommentToPersonCommand(
    string PersonId,
    string Content,
    string UserId
) : IRequest<CommentDTO>;

public class AddCommentToPersonCommandHandler : IRequestHandler<AddCommentToPersonCommand, CommentDTO>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;

    public AddCommentToPersonCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<CommentDTO> Handle(AddCommentToPersonCommand request, CancellationToken cancellationToken)
    {
        Guid userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        Guid personGuid = request.PersonId.ToGuid();
        DomainPerson targetPerson = await _repository.FindByIdAsync<DomainPerson>(personGuid, cancellationToken)
            ?? throw new KeyNotFoundException("There is no person object with the given id value.");

        DateTime now = DateTime.UtcNow;
        DomainComment comment = new(user, now)
        {
            PersonId = targetPerson.Id,
            Content = request.Content,
        };
        targetPerson.Comments.Add(comment);
        await _uow.CommitAsync(cancellationToken);

        return comment.ToDTO(0);
    }
}