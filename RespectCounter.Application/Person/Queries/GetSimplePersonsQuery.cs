using MediatR;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.Contracts;
using DomainPerson = RespectCounter.Domain.Model.Person;

namespace RespectCounter.Application.Person.Queries;

public record GetSimplePersonsQuery() : IRequest<IEnumerable<SimplePersonDTO>>;

public class GetSimplePersonsQueryHandler : IRequestHandler<GetSimplePersonsQuery, IEnumerable<SimplePersonDTO>>
{
    private readonly IReadOnlyRepository _repository;
    
    public GetSimplePersonsQueryHandler(IReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SimplePersonDTO>> Handle(GetSimplePersonsQuery request, CancellationToken cancellationToken)
    {
        var persons = await _repository.FindListAsync<DomainPerson>(cancellationToken);
        return persons.Select(p => p.ToSimpleDTO());
    }
}