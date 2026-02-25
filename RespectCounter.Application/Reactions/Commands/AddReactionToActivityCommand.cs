using MediatR;
using RespectCounter.Domain.Enums;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Reactions.Commands;

public record AddReactionToActivityCommand(
    string ActivityId,
    int ReactionType,
    string UserId
) : IRequest<int>;

public class AddReactionToActivityCommandHandler : IRequestHandler<AddReactionToActivityCommand, int>
{
    private readonly IReadOnlyRepository _roRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IReactionRepository _reactionRepostiory;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public AddReactionToActivityCommandHandler(
        IReadOnlyRepository roRepository, 
        IActivityRepository activityRepository,
        IReactionRepository reactionRepository,
        IUnitOfWork uow, 
        IIdentityService userService)
    {
        _roRepository = roRepository;
        _activityRepository = activityRepository;
        _reactionRepostiory = reactionRepository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<int> Handle(AddReactionToActivityCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _roRepository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var activityId = request.ActivityId.ToGuid();
        var targetActivity = await _activityRepository.FindByIdAsync(activityId, cancellationToken)
            ?? throw new InvalidOperationException($"The Activity with ID {request.ActivityId} was not found in the system, despite the previous validation check.");

        DateTime now = DateTime.UtcNow;
        ActivityReaction? reaction = await _reactionRepostiory.TryGetCurrentUserReactionForActivityAsync(activityId, userId, cancellationToken);
        if(reaction != null)
        { 
            reaction.ReactionType = (ReactionType) request.ReactionType;
            reaction.Updated(user, now);
        }
        else
        {
            reaction = new ActivityReaction(targetActivity, (ReactionType)request.ReactionType, user, now);
            targetActivity.Reactions.Add(reaction);
        }
        await _uow.CommitAsync(cancellationToken);
        
        var result = targetActivity.Reactions.Sum(r => (int)r.ReactionType);
        return result;
    }
}