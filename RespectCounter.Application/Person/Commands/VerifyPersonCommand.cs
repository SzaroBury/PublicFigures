using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using PersonDomain = RespectCounter.Domain.Model.Person;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Person.Commands
{
    public record VerifyPersonCommand(
        string PersonId,
        string UserId
    ) : IRequest<PersonDTO>;

    public class VerifyPersonCommandHandler : IRequestHandler<VerifyPersonCommand, PersonDTO>
    {
        private readonly IReadOnlyRepository _roRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _uow;

        public VerifyPersonCommandHandler(IReadOnlyRepository roRepository, IPersonRepository personRepository, IUnitOfWork uow)
        {
            _roRepository = roRepository;
            _personRepository = personRepository;
            _uow = uow;
        }

        public async Task<PersonDTO> Handle(VerifyPersonCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId.ToGuid();
            var user = await _roRepository.FindByIdAsync<User>(userId, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

            DateTime now = DateTime.UtcNow;

            Guid personGuid = request.PersonId.ToGuid();
            PersonDomain? p = await _personRepository.GetByIdAsync(personGuid, cancellationToken)
                ?? throw new InvalidOperationException($"Person with ID {personGuid} not found, despite prior validation.");
                
            p.Verify(user, now);
            await _uow.CommitAsync(cancellationToken);

            return p.ToDTO(null);
        }
    }
}