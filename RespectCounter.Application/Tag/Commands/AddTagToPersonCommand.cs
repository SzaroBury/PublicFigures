using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Contracts;
using DomainPerson = RespectCounter.Domain.Model.Person;
using DomainTag = RespectCounter.Domain.Model.Tag;

namespace RespectCounter.Application.Tag.Commands;

public record AddTagToPersonCommand(
    string PersonId,
    string TagName,
    string UserId
) : IRequest<PersonDTO>;

public class AddTagToPersonCommandHandler : IRequestHandler<AddTagToPersonCommand, PersonDTO>
{
    private readonly IReadOnlyRepository _roRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _uow;

    public AddTagToPersonCommandHandler(
        IReadOnlyRepository roRepository, 
        IPersonRepository personRepository,
        ITagRepository tagRepository,
        IUnitOfWork uow)
    {
        _roRepository = roRepository;
        _personRepository = personRepository;
        _tagRepository = tagRepository;
        _uow = uow;
    }

    public async Task<PersonDTO> Handle(AddTagToPersonCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _roRepository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var personId = request.PersonId.ToGuid();
        DomainPerson targetPerson = await _personRepository.GetByIdAsync(personId, cancellationToken)
            ?? throw new InvalidOperationException($"The Person with ID {request.PersonId} was not found in the system, despite the previous validation check.");

        var now = DateTime.UtcNow;
        DomainTag? tag = await _tagRepository.GetByNameAsync(request.TagName.ToLower(), cancellationToken);
        if(tag == null)
        {
            tag = new(user, now)
            {
                Name = request.TagName,
                Description = $"Created for {targetPerson.FirstName} {targetPerson.LastName} person object."
            };
            await _tagRepository.AddTagAsync(tag, cancellationToken);
        }
        else if(targetPerson.Tags.Any(pt => pt.Tag.Name.ToLower() == tag.Name.ToLower()))
        {
            throw new InvalidOperationException("The pointed person already has the given tag.");
        }

        PersonTag personTag = new(targetPerson, tag, user, now);
        targetPerson.Tags.Add(personTag);

        await _uow.CommitAsync(cancellationToken);
        return targetPerson.ToDTO(userId);
    }
}