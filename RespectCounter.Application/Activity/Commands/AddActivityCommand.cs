using System.Globalization;
using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.DTOs;
using DomainActivity = RespectCounter.Domain.Model.Activity;
using DomainPerson = RespectCounter.Domain.Model.Person;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Activity.Commands;

public record AddActivityCommand(
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

public class AddActivityCommandHandler : IRequestHandler<AddActivityCommand, ActivityDTO>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;

    public AddActivityCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<ActivityDTO> Handle(AddActivityCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserAsync(request.UserId, cancellationToken);
        DateTime now = DateTime.UtcNow;
        var newActivity = await CreateActivityAsync(request, user, now, cancellationToken);
        await AddTagsToActivityAsync(request.Tags, user, now, newActivity, cancellationToken);
        await SaveActivityAsync(newActivity, cancellationToken);
        return newActivity.ToDTO(user.Id);
    }

    public async Task<User> GetUserAsync(string userId, CancellationToken cancellationToken)
    {
        Guid userGuid = userId.ToGuid();
        return await _repository.FindByIdAsync<User>(userGuid, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");
    }

    public async Task<DomainActivity> CreateActivityAsync(AddActivityCommand request, User user, DateTime now, CancellationToken cancellationToken)
    {
        Guid personGuid = Guid.Parse(request.PersonId);
        DomainPerson person = await _repository.FindByIdAsync<DomainPerson>(personGuid, cancellationToken)
            ?? throw new InvalidOperationException($"The Person with ID {request.PersonId} was not found in the system, despite the previous validation check.");

        DateTime? happend = null;
        if (!string.IsNullOrEmpty(request.Happend))
        {
            _ = DateTime.TryParseExact(request.Happend, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate);
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

    private async Task AddTagsToActivityAsync(string tagsString, User user, DateTime now, DomainActivity newActivity, CancellationToken cancellationToken)
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

    private async Task SaveActivityAsync(DomainActivity newActivity, CancellationToken cancellationToken)
    {
        _uow.GetWriteRepository().Add(newActivity);
        await _uow.CommitAsync(cancellationToken);
    }
}