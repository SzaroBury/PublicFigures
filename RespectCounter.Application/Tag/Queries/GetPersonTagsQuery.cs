using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainPerson = RespectCounter.Domain.Model.Person;

namespace RespectCounter.Application.Tags.Queries
{
    public record GetPersonTagsQuery(string PersonId, int AtLeastCount = 1) : IRequest<IEnumerable<SimpleTagDTO>>;

    public class GetPersonTagsQueryHandler : IRequestHandler<GetPersonTagsQuery, IEnumerable<SimpleTagDTO>>
    {
        private readonly IReadOnlyRepository _repository;
        
        public GetPersonTagsQueryHandler(IReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SimpleTagDTO>> Handle(GetPersonTagsQuery request, CancellationToken cancellationToken)
        {
            var person = await _repository.SingleOrDefaultAsync<DomainPerson>(
                p => p.Id.ToString() == request.PersonId,
                "Tags.Tag.Persons,Tags.Tag.Activities"
            ) ?? throw new InvalidOperationException($"The Person with ID {request.PersonId} was not found in the system, despite the previous validation check.");

            var tags = person.Tags.Where(t => t.Tag.Count >= request.AtLeastCount).Select(pt => pt.Tag.ToSimpleDTO());
            return tags;
        }
    }
}