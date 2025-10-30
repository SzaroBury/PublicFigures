using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainActivity = RespectCounter.Domain.Model.Activity;
using DomainComment = RespectCounter.Domain.Model.Comment;

namespace RespectCounter.Application.Comment.Commands;

public record AddCommentToActivityCommand(
    string ActivityId,
    string Content,
    string UserId
) : IRequest<ActivityDTO>;

public class AddCommentToActivityCommandHandler : IRequestHandler<AddCommentToActivityCommand, ActivityDTO>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;

    public AddCommentToActivityCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<ActivityDTO> Handle(AddCommentToActivityCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var activityId = request.ActivityId.ToGuid();
        var activity = await _repository.FindByIdAsync<DomainActivity>(activityId, cancellationToken)
            ?? throw new KeyNotFoundException("There is no activity object with the given id value.");

        DateTime now = DateTime.UtcNow;
        DomainComment comment = new(user, now)
        {
            ActivityId = activityId,
            Content = request.Content
        };
        activity.Comments.Add(comment);
        await _uow.CommitAsync(cancellationToken);
        return activity.ToDTO(userId);
    }
}