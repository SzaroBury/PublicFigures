using MediatR;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainPerson = RespectCounter.Domain.Model.Person;
using DomainComment = RespectCounter.Domain.Model.Comment;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Person.Queries
{
    public record GetPersonByIdQuery(string PersonId, string? UserId) : IRequest<PersonDTO>;

    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDTO>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IIdentityService _userService;

        public GetPersonByIdQueryHandler(IPersonRepository personRepository, IIdentityService userService)
        {
            _personRepository = personRepository;
            _userService = userService;
        }

        public async Task<PersonDTO> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            Guid? userId = request.UserId.ToNullableGuid();

            var personId = request.PersonId.ToGuid();
            var person = await _personRepository.GetByIdAsync(personId, cancellationToken)
                ?? throw new KeyNotFoundException("The person was not found. Please enter Id of an existing person.");

            return person.ToDTO(userId);
        }
    }
}