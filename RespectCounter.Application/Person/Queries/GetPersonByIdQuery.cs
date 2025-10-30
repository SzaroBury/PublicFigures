using MediatR;
using RespectCounter.Domain.Contracts;
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
        private readonly IReadOnlyRepository _repository;
        private readonly IIdentityService _userService;

        public GetPersonByIdQueryHandler(IReadOnlyRepository repository, IIdentityService userService)
        {
            _repository = repository;
            _userService = userService;
        }

        public async Task<PersonDTO> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            Guid? userId = request.UserId.ToNullableGuid();

            var personId = request.PersonId.ToGuid();
            var person = await _repository.SingleOrDefaultAsync<DomainPerson>(
                    a => a.Id == personId,
                    "Activities,Comments.Children,Reactions,Tags,CreatedBy,LastUpdatedBy",
                    cancellationToken
                ) ?? throw new KeyNotFoundException("The person was not found. Please enter Id of an existing person.");


            var comments = await _repository.FindListAsync<DomainComment>(c => c.PersonId == person.Id);

            return person.ToDTO(userId);
        }
    }
}