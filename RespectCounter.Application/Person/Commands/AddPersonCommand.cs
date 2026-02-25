using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.Contracts;
using DomainPerson = RespectCounter.Domain.Model.Person;
using DomainTag = RespectCounter.Domain.Model.Tag;

namespace RespectCounter.Application.Person.Commands;

public record AddPersonCommand(
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

public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, PersonDTO>
{
    private readonly IReadOnlyRepository _repository;
    private readonly ITagRepository _tagRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public AddPersonCommandHandler(IReadOnlyRepository repository, ITagRepository tagRepository, IPersonRepository personRepository, IUnitOfWork uow, IIdentityService userService)
    {
        _repository = repository;
        _tagRepository = tagRepository;
        _personRepository = personRepository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<PersonDTO> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        DateOnly? birthday = null;
        if (!string.IsNullOrWhiteSpace(request.Birthday))
        {
            birthday = request.Birthday.ToDateOnly();
        }

        DateOnly? deathDate = null;
        if (!string.IsNullOrWhiteSpace(request.DeathDate))
        {
            deathDate = request.DeathDate.ToDateOnly();
        } 

        DateTime now = DateTime.UtcNow;
        DomainPerson newPerson = new(user, now)
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            NickName = request.NickName,
            Profession = request.Profession,
            Description = request.Description,
            Nationality = request.Nationality,
            Birthday = birthday,
            DeathDate = deathDate,
            Status = PersonStatus.NotVerified,
        };

        foreach (string tagName in request.Tags)
        {
            DomainTag? tag = await _tagRepository.GetByNameAsync(tagName, cancellationToken);
            if (tag == null)
            {
                tag = new(user, now)
                {
                    Name = tagName,
                    Description = $"Created with {request.FirstName} {request.LastName} person object."
                };
                await _tagRepository.AddTagAsync(tag, cancellationToken);
            }
            PersonTag personTag = new(newPerson, tag, user, now);
            newPerson.Tags.Add(personTag);
        }
        await _personRepository.AddAsync(newPerson, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        return newPerson.ToDTO(userId);
    }
}
