using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.Contracts;
using DomainPerson = RespectCounter.Domain.Model.Person;

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
    IEnumerable<string> Tags,
    string UserId
) : IRequest<PersonDTO>;

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, PersonDTO>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IPersonRepository _personRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public UpdatePersonCommandHandler(IReadOnlyRepository repository, IPersonRepository personRepository, ITagRepository tagRepository, IUnitOfWork uow, IIdentityService userService)
    {
        _repository = repository;
        _personRepository = personRepository;
        _tagRepository = tagRepository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<PersonDTO> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var personGuid = request.PersonId.ToGuid();
        var person = await _personRepository.GetByIdAsync(personGuid, cancellationToken)
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


        var updatedTags = request.Tags.Select(t => t.ToLower());
        var currentPersonTags = person.Tags.ToDictionary(pt => pt.Tag.Name.ToLower(), pt => pt);
        var currentTagNames = currentPersonTags.Keys.ToList();

        var tagsToRemove = currentTagNames
            .Except(updatedTags)
            .ToList();

        var tagsToAdd = updatedTags
            .Except(currentTagNames)
            .ToList();

        foreach (var tagName in tagsToRemove)
        {
            if (currentPersonTags.TryGetValue(tagName, out var personTagToRemove))
            {
                person.Tags.Remove(personTagToRemove);
            }
        }

        if (tagsToAdd.Count != 0)
        {
            var existingTags = await _tagRepository.GetByNamesAsync(tagsToAdd, cancellationToken);
            var existingTagNames = existingTags.Select(t => t.Name.ToLower()).ToHashSet();

            foreach (var newTagName in tagsToAdd)
            {
                Domain.Model.Tag? tagToAdd = existingTags.FirstOrDefault(t => t.Name.Equals(newTagName, StringComparison.OrdinalIgnoreCase));

                if (tagToAdd is null)
                {
                    tagToAdd = new Domain.Model.Tag(user, now)
                    {
                        Name = newTagName,
                        Description = $"Created during update of {request.FirstName} {request.LastName}."
                    };
                    await _tagRepository.AddTagAsync(tagToAdd, cancellationToken);
                }

                var newPersonTag = new PersonTag(person, tagToAdd, user, now);
                person.Tags.Add(newPersonTag);
            }
        }

        await _uow.CommitAsync(cancellationToken);
        return person.ToDTO(userId);
    }
}
