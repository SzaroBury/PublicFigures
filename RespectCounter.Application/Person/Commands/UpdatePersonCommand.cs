using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainPerson = RespectCounter.Domain.Model.Person;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Person.Commands;

public record UpdatePersonCommand(
    string PersonId,
    string FirstName, 
    string LastName, 
    string NickName,
    string Profession,
    string Description, 
    string Nationality, 
    string? Birthday, 
    string? DeathDate, 
    string Tags,
    string UserId
) : IRequest<PersonDTO>;

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, PersonDTO>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public UpdatePersonCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow, IIdentityService userService)
    {
        _repository = repository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<PersonDTO> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var personGuid = request.PersonId.ToGuid();
        var person = await _repository.SingleOrDefaultAsync<DomainPerson>(p => p.Id == personGuid, "Tags.Tag", cancellationToken)
            ?? throw new InvalidOperationException($"Person with ID {personGuid} not found, despite prior validation.");

        var now = DateTime.UtcNow;

        person.FirstName = request.FirstName;
        person.LastName = request.LastName;
        person.NickName = request.NickName;
        person.Profession = request.Profession;
        person.Description = request.Description;
        person.Nationality = request.Nationality;
        person.Birthday = request.Birthday?.ToDateOnly();
        person.DeathDate = request.DeathDate?.ToDateOnly();
        person.Updated(user, now);


        var newTagNames = request.Tags.ToLower()
                                     .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(t => t.Trim())
                                     .Distinct()
                                     .ToList();

        var currentPersonTags = person.Tags.ToDictionary(pt => pt.Tag.Name.ToLower(), pt => pt);
        var currentTagNames = currentPersonTags.Keys.ToList();

        var tagsToRemove = currentTagNames
            .Except(newTagNames)
            .ToList();

        var tagsToAdd = newTagNames
            .Except(currentTagNames)
            .ToList();

        foreach (var tagName in tagsToRemove)
        {
            if (currentPersonTags.TryGetValue(tagName, out var personTagToRemove))
            {
                person.Tags.Remove(personTagToRemove);
                _uow.GetWriteRepository().Update(personTagToRemove);
            }
        }

        if (tagsToAdd.Any())
        {
            var existingTags = await _repository.FindListAsync<Tag>(
                t => tagsToAdd.Contains(t.Name.ToLower()), 
                cancellationToken: cancellationToken);

            var existingTagNames = existingTags.Select(t => t.Name.ToLower()).ToHashSet();

            foreach (var newTagName in tagsToAdd)
            {
                Tag? tagToAdd = existingTags.FirstOrDefault(t => t.Name.Equals(newTagName, StringComparison.OrdinalIgnoreCase));

                if (tagToAdd is null)
                {
                    tagToAdd = new Tag(user, now)
                    {
                        Name = newTagName,
                        Description = $"Created during update of {request.FirstName} {request.LastName}."
                    };
                    _uow.GetWriteRepository().Add(tagToAdd);
                }

                var newPersonTag = new PersonTag(person, tagToAdd, user, now);
                person.Tags.Add(newPersonTag);
            }
        }

        _uow.GetWriteRepository().Update(person);
        await _uow.CommitAsync(cancellationToken);

        return person.ToDTO(userId);
    }
}
