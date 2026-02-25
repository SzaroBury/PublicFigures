using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainComment = RespectCounter.Domain.Model.Comment;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Comment.Commands;

public record AddCommentToActivityCommand(
    string ActivityId,
    string Content,
    string UserId
) : IRequest<ActivityDTO>;

public class AddCommentToActivityCommandHandler : IRequestHandler<AddCommentToActivityCommand, ActivityDTO>
{
    private readonly IReadOnlyRepository _readOnlyRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IUnitOfWork _uow;

    public AddCommentToActivityCommandHandler(IReadOnlyRepository readOnlyRepository, IActivityRepository activityRepository, IUnitOfWork uow)
    {
        _readOnlyRepository = readOnlyRepository;
        _activityRepository = activityRepository;
        _uow = uow;
    }

    public async Task<ActivityDTO> Handle(AddCommentToActivityCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _readOnlyRepository.FindByIdAsync<User>(userId, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var activityId = request.ActivityId.ToGuid();
        var activity = await _activityRepository.FindByIdAsync(activityId, cancellationToken)
            ?? throw new KeyNotFoundException("There is no activity object with the given id value.");

        DateTime now = DateTime.UtcNow;
        DomainComment comment = new(user, now)
        {
            ActivityId = activityId,
            Content = request.Content
        };
        activity.Comments.Add(comment);
        
        _activityRepository.Update(activity);
        await _uow.CommitAsync(cancellationToken);
        return activity.ToDTO(userId);
    }
}