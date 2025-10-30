using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
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
    private readonly IReadOnlyRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IIdentityService _userService;

    public AddReactionToPersonCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow, IIdentityService userService)
    {
        _repository = repository;
        _uow = uow;
        _userService = userService;
    }

    public async Task<int> Handle(AddReactionToPersonCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToGuid();
        var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
            ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

        var personId = request.PersonId.ToGuid();
        DomainPerson? targetPerson = await _repository.SingleOrDefaultAsync<DomainPerson>(p => p.Id == personId, "Reactions", cancellationToken)
            ?? throw new InvalidOperationException($"The Person with ID {request.PersonId} was not found in the system, despite the previous validation check.");

        PersonReaction? reaction = _repository.FindQueryable<PersonReaction>(r => r.PersonId == personId && r.CreatedById == user.Id).FirstOrDefault();
        DateTime now = DateTime.UtcNow;
        if(reaction != null) 
        {
            //throw new InvalidOperationException("This user has already reacted to this person.");
            reaction.ReactionType = (ReactionType) request.ReactionType;
            reaction.LastUpdated = now;
            _uow.GetWriteRepository().Update(reaction);
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