using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Enums;
using DomainPerson = RespectCounter.Domain.Model.Person;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Reactions.Commands;

public record AddReactionToPersonCommand(
    string PersonId,
    int ReactionType,
    string UserId
) : IRequest<int>;

public class AddReactionToPersonCommandHandler : IRequestHandler<AddReactionToPersonCommand, int>
{
    private readonly IReadOnlyRepository _roRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public AddReactionToPersonCommandHandler(
        IReadOnlyRepository roRepository, 
        IPersonRepository personRepository,
        IReactionRepository reactionRepository,
        IUnitOfWork uow, 
        IIdentityService userService)
    {
        _roRepository = roRepository;
        _personRepository = personRepository;
        _reactionRepository = reactionRepository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<int> Handle(AddReactionToPersonCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _roRepository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var personId = request.PersonId.ToGuid();
        DomainPerson? targetPerson = await _personRepository.GetByIdAsync(personId, cancellationToken)
            ?? throw new InvalidOperationException($"The Person with ID {request.PersonId} was not found in the system, despite the previous validation check.");

        DateTime now = DateTime.UtcNow;
        PersonReaction? reaction = await _reactionRepository.TryGetCurrentUserReactionForPersonAsync(personId, user.Id);
        if(reaction != null) 
        {
            reaction.ReactionType = (ReactionType) request.ReactionType;
            reaction.Updated(user, now);
        }
        else
        {
            reaction = new PersonReaction(targetPerson, (ReactionType)request.ReactionType, user, now);
            targetPerson.Reactions.Add(reaction);
        }

        await _uow.CommitAsync(cancellationToken);
        var result = targetPerson.Reactions.Sum(r => (int)r.ReactionType);
        return result;
    }
}