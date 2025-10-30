using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Enums;
using DomainActivity = RespectCounter.Domain.Model.Activity;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Reactions.Commands;

public record AddReactionToActivityCommand(
    string ActivityId,
    int ReactionType,
    string UserId
) : IRequest<int>;

public class AddReactionToActivityCommandHandler : IRequestHandler<AddReactionToActivityCommand, int>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public AddReactionToActivityCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow, IIdentityService userService)
    {
        _repository = repository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<int> Handle(AddReactionToActivityCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var activityId = request.ActivityId.ToGuid();
        var targetActivity = await _repository.SingleOrDefaultAsync<DomainActivity>(
            a => a.Id == activityId,
            "Reactions",
            cancellationToken)
            ?? throw new InvalidOperationException($"The Activity with ID {request.ActivityId} was not found in the system, despite the previous validation check.");

        ActivityReaction? reaction = _repository.FindQueryable<ActivityReaction>(r => r.ActivityId == activityId && r.CreatedById == user.Id).FirstOrDefault();

        DateTime now = DateTime.UtcNow;
        if(reaction != null)
        { 
            // throw new InvalidOperationException("This user has already reacted to this activity.");
            reaction.ReactionType = (ReactionType) request.ReactionType;
            reaction.LastUpdated = now;
            _uow.GetWriteRepository().Update(reaction);
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