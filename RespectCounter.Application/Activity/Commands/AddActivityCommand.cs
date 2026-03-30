using MediatR;
using System.Globalization;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Contracts;

using DomainActivity = RespectCounter.Domain.Model.Activity;
using DomainPerson   = RespectCounter.Domain.Model.Person;
using DomainTag      = RespectCounter.Domain.Model.Tag;

namespace RespectCounter.Application.Activity.Commands;

public record AddActivityCommand(
    string PersonId,
    string Value, 
    string Description, 
    string Location, 
    string Happend, 
    string Source, 
    int Type, 
    IEnumerable<string> Tags,
    string UserId
) : IRequest<ActivityDTO>;

public class AddActivityCommandHandler : IRequestHandler<AddActivityCommand, ActivityDTO>
{
    private readonly IReadOnlyRepository _readOnlyRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IUnitOfWork _uow;

    public AddActivityCommandHandler(
        IReadOnlyRepository readOnlyRepository, 
        ITagRepository tagRepository, 
        IActivityRepository activityRepository, 
        IUnitOfWork uow)
    {
        _readOnlyRepository = readOnlyRepository;
        _tagRepository = tagRepository;
        _activityRepository = activityRepository;
        _uow = uow;
    }

    public async Task<ActivityDTO> Handle(AddActivityCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserAsync(request.UserId, cancellationToken);
        var person = await GetPersonAsync(request.PersonId, cancellationToken);

        DateTime now = DateTime.UtcNow;
        var newActivity = CreateActivity(request, user, person, now, cancellationToken);
        var tags = await GetAndCreateNonExistingTagsAsync(request.Tags, user, now, newActivity.Id, cancellationToken);
        AssignTagsToActivity(newActivity, tags, user, now);
        await SaveActivityAsync(newActivity, cancellationToken);
        return newActivity.ToDTO(user.Id);
    }

    public async Task<User> GetUserAsync(string userId, CancellationToken cancellationToken)
    {
        Guid userGuid = userId.ToGuid();
        return await _readOnlyRepository.FindByIdAsync<User>(userGuid, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");
    }

    public async Task<DomainPerson> GetPersonAsync(string personId, CancellationToken cancellationToken)
    {
        Guid personGuid = Guid.Parse(personId);
        return await _readOnlyRepository.FindByIdAsync<DomainPerson>(personGuid, cancellationToken)
            ?? throw new InvalidOperationException($"The Person with ID {personId} was not found in the system, despite the previous validation check.");
    }

    public DomainActivity CreateActivity(AddActivityCommand request, User user, DomainPerson person, DateTime now, CancellationToken cancellationToken)
    {
        DateTime? happend = null;
        if (!string.IsNullOrEmpty(request.Happend))
        {
            _ = DateTime.TryParseExact(
                request.Happend, 
                "yyyy-MM-ddTHH:mm:ss.fffZ", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, 
                out DateTime parsedDate
            );
            happend = parsedDate;
        }

        return new(user, now)
        {
            Value = request.Value,
            Location = request.Location,
            Description = request.Description,
            Source = request.Source,
            Type = (ActivityType)request.Type,
            Happend = happend,
            Person = person
        };
    }

    private async Task<IEnumerable<DomainTag>> GetAndCreateNonExistingTagsAsync(IEnumerable<string> inputTags, User user, DateTime now, Guid activityId, CancellationToken cancellationToken)
    {
        var uniqueNames = inputTags
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.Trim().ToLower())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var existingTags = await _tagRepository.GetByNamesWithoutTrackingAsync(uniqueNames, cancellationToken);
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

    private void AssignTagsToActivity(DomainActivity activity, IEnumerable<DomainTag> tags, User user, DateTime now)
    {
        foreach(DomainTag tag in tags)
        {
            var activityTag = new ActivityTag(activity, tag, user, now);
            activity.Tags.Add(activityTag);
        }
    }

    private async Task SaveActivityAsync(DomainActivity newActivity, CancellationToken cancellationToken)
    {
        _activityRepository.Add(newActivity);
        await _uow.CommitAsync(cancellationToken);
    }
}