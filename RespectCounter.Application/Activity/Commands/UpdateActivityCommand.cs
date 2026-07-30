using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainActivity = RespectCounter.Domain.Model.Activity;
using DomainTag = RespectCounter.Domain.Model.Tag;

namespace RespectCounter.Application.Activity.Commands;

public record UpdateActivityCommand(
    string ActivityId,
    string PersonId,
    string Value, 
    string Description, 
    string Location, 
    string OccurredAt, 
    string Source, 
    int Type, 
    IEnumerable<string> Tags,
    string UserId
) : IRequest<ActivityDTO>;

public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, ActivityDTO>
{
    private readonly IReadOnlyRepository _readOnlyRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _uow;

    public UpdateActivityCommandHandler(
        IReadOnlyRepository readOnlyRepository, 
        IActivityRepository activityRepository, 
        ITagRepository tagRepository, 
        IUnitOfWork uow)
    {
        _readOnlyRepository = readOnlyRepository;
        _activityRepository = activityRepository;
        _tagRepository = tagRepository;
        _uow = uow;
    }

    public async Task<ActivityDTO> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
    {
        Guid userId = request.UserId.ToGuid();
        var user = await _readOnlyRepository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        Guid personId = request.PersonId.ToGuid();
        if(!await _readOnlyRepository.ExistsAsync<Domain.Model.Person>(personId, cancellationToken))
            throw new InvalidOperationException($"The Person with ID {personId} does not exist in the system, despite the previous validation check.");

        Guid activityGuid = Guid.Parse(request.ActivityId);
        var activity = await _activityRepository.FindByIdAsync(activityGuid, cancellationToken)
            ?? throw new InvalidOperationException($"The Activity with ID {request.ActivityId} was not found in the system, despite the previous validation check.");

        DateTime now = DateTime.UtcNow;

        var tags = await GetAndCreateTagsAsync(request.Tags, user, now, activity.Id, cancellationToken);
        AssignAndRemoveTags(activity, tags, user, now);
        await SaveActivityAsync(activity, cancellationToken);
        return activity.ToDTO(user.Id);
    }

    private async Task<IEnumerable<DomainTag>> GetAndCreateTagsAsync(IEnumerable<string> inputTags, User user, DateTime now, Guid activityId, CancellationToken cancellationToken)
    {
        var uniqueNames = inputTags
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var uniqueNamesLowercase = uniqueNames.Select(t => t.ToLower());

        var existingTags = await _tagRepository.GetByNamesWithoutTrackingAsync(uniqueNamesLowercase, cancellationToken);
        var existingNamesSet = new HashSet<string>(existingTags.Select(t => t.Name), StringComparer.OrdinalIgnoreCase);

        var tagsToCreate = uniqueNames
            .Where(t => !existingNamesSet.Contains(t))
            .Select(tag => new DomainTag(user, now) { Name = tag, Description = $"Created with '{activityId}' activity object." })
            .ToList();

        foreach (DomainTag tag in tagsToCreate)
        {
            _tagRepository.AddTag(tag);
        }

        return existingTags.Union(tagsToCreate);
    }

    private void AssignAndRemoveTags(DomainActivity activity, IEnumerable<DomainTag> desiredTags, User user, DateTime now)
    {
        var tagsToAdd = desiredTags
            .Where(dt => activity.Tags.Any(at => at.TagId == dt.Id))
            .Select(t => new ActivityTag(activity, t, user, now))
            .ToList();
        foreach(ActivityTag tag in tagsToAdd)
        {
            activity.Tags.Add(tag);
        }
        
        var tagsToRemove = activity.Tags
            .Where(at => !desiredTags.Any(t => t.Id == at.TagId))
            .ToList();
        foreach(ActivityTag tag in tagsToRemove)
        {
            activity.Tags.Remove(tag);
        }
    }

    private async Task SaveActivityAsync(DomainActivity updatedActivity, CancellationToken cancellationToken)
    {
        await _uow.CommitAsync(cancellationToken);
    }
}