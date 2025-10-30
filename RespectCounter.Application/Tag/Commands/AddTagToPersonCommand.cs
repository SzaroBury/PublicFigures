using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.Extensions;
using DomainPerson = RespectCounter.Domain.Model.Person;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Tags.Commands;

public record AddTagToPersonCommand(
    string PersonId,
    string TagName,
    string UserId
) : IRequest<PersonDTO>;

public class AddTagToPersonCommandHandler : IRequestHandler<AddTagToPersonCommand, PersonDTO>
{
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public AddTagToPersonCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow, IIdentityService userService)
    {
        _repository = repository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<PersonDTO> Handle(AddTagToPersonCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var personId = request.PersonId.ToGuid();
        DomainPerson targetPerson = await _repository.SingleOrDefaultAsync<DomainPerson>(p => p.Id == personId, "Tags")
            ?? throw new InvalidOperationException($"The Person with ID {request.PersonId} was not found in the system, despite the previous validation check.");

        Tag? existingTag = _repository.FindQueryable<Tag>(t => t.Name.ToLower() == request.TagName.ToLower()).FirstOrDefault();
        var now = DateTime.UtcNow;
        if(existingTag == null)
        {
            Tag newTag = new(user, now)
            {
                Name = request.TagName,
                Description = $"Created for {targetPerson.FirstName} {targetPerson.LastName} person object."
            };
            existingTag = _uow.GetWriteRepository().Add(newTag);
        }
        else if(targetPerson.Tags.Any(pt => pt.Tag.Name.ToLower() == existingTag.Name.ToLower()))
        {
            throw new InvalidOperationException("The pointed person already has the given tag.");
        }
        PersonTag personTag = new(targetPerson, existingTag, user, now);
        targetPerson.Tags.Add(personTag);

        await _uow.CommitAsync(cancellationToken);

        return targetPerson.ToDTO(userId);
    }
}