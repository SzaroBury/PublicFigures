using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
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
        private IReadOnlyRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IIdentityService _userService;

        public VerifyPersonCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow, IIdentityService userService)
        {
            _repository = repository;
            _uow = uow;
            _userService = userService;
        }

        public async Task<PersonDTO> Handle(VerifyPersonCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId.ToGuid();
            var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

            DateTime now = DateTime.UtcNow;

            Guid personGuid = request.PersonId.ToGuid();
            PersonDomain? p = await _repository.FindByIdAsync<PersonDomain>(personGuid, cancellationToken)
                ?? throw new InvalidOperationException($"Person with ID {personGuid} not found, despite prior validation.");
                
            p.Verify(user, now);
            await _uow.CommitAsync(cancellationToken);

            return p.ToDTO(null);
        }
    }
}