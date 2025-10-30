using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainPerson = RespectCounter.Domain.Model.Person;
using RespectCounter.Domain.Enums;

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
        var persons = await _repository.FindListAsync<DomainPerson>(
            p => p.Status != PersonStatus.Hidden,
            null,
            q => q.OrderByDescending(c => c.Created),
            cancellationToken
        );
        return persons.Select(p => p.ToSimpleDTO());
    }
}