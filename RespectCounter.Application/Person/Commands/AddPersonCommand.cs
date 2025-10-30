using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using PersonDomain = RespectCounter.Domain.Model.Person;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Shared.Contracts;

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
    string Tags,
    string UserId
) : IRequest<PersonDTO>;

public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, PersonDTO>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public AddPersonCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow, IIdentityService userService)
    {
        _repository = repository;
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
        PersonDomain newPerson = new(user, now)
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

        List<string> tags = request.Tags.Split(",").ToList();
        tags.Remove("");
        foreach (string tag in tags)
        {
            Tag? existingTag = _repository.FindQueryable<Tag>(t => t.Name.ToLower() == tag.ToLower()).FirstOrDefault();
            if (existingTag == null)
            {
                Tag newTag = new(user, now)
                {
                    Name = tag,
                    Description = $"Created with {request.FirstName} {request.LastName} person object."
                };
                existingTag = _uow.GetWriteRepository().Add(newTag);
            }
            PersonTag personTag = new(newPerson, existingTag, user, now);
            newPerson.Tags.Add(personTag);
        }
        var result = _uow.GetWriteRepository().Add(newPerson);
        await _uow.CommitAsync(cancellationToken);

        return result.ToDTO(userId);
    }
}
