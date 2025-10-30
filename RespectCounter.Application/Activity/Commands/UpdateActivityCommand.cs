using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.DTOs;
using DomainActivity = RespectCounter.Domain.Model.Activity;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Activity.Commands;

public record UpdateActivityCommand(
    string ActivityId,
    string PersonId,
    string Value, 
    string Description, 
    string Location, 
    string Happend, 
    string Source, 
    int Type, 
    string Tags,
    string UserId
) : IRequest<ActivityDTO>;

public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, ActivityDTO>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public UpdateActivityCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow, IIdentityService userService)
    {
        _repository = repository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<ActivityDTO> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
    {
        Guid userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        Guid activityGuid = Guid.Parse(request.ActivityId);
        var activity = await _repository.FindByIdAsync<DomainActivity>(activityGuid, cancellationToken)
            ?? throw new InvalidOperationException($"The Activity with ID {request.ActivityId} was not found in the system, despite the previous validation check.");

        DateTime now = DateTime.UtcNow;

        await UpdateTagsAsync(request.Tags, user, now, activity, cancellationToken);
        await SaveActivityAsync(activity, cancellationToken);
        return activity.ToDTO(user.Id);
    }

    private async Task UpdateTagsAsync(string tagsString, User user, DateTime now, DomainActivity newActivity, CancellationToken cancellationToken)
    {
        List<string> tags = tagsString.Split(",").ToList();
        foreach (string tag in tags)
        {
            Tag? existingTag = await _repository.SingleOrDefaultAsync<Tag>(t => t.Name.ToLower() == tag.ToLower(), cancellationToken: cancellationToken);
            if (existingTag == null)
            {
                Tag newTag = new Tag()
                {
                    Name = tag,
                    Description = $"Created with '{newActivity.Id}' activity object.",
                    Created = now,
                    CreatedById = Guid.Empty,
                    LastUpdated = now,
                    LastUpdatedById = Guid.Empty
                };
                existingTag = _uow.GetWriteRepository().Add(newTag);
            }
            ActivityTag activityTag = new(newActivity, existingTag, user, now);
            newActivity.Tags.Add(activityTag);
        }
    }

    private async Task SaveActivityAsync(DomainActivity updatedActivity, CancellationToken cancellationToken)
    {
        _uow.GetWriteRepository().Update(updatedActivity);
        await _uow.CommitAsync(cancellationToken);
    }
}