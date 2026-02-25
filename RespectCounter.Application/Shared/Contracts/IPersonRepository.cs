using RespectCounter.Application.Shared.Enums;
using RespectCounter.Domain.Enums;
using DomainPerson = RespectCounter.Domain.Model.Person;

namespace RespectCounter.Application.Shared.Contracts;

public interface IPersonRepository
{
    Task<PagedResult<DomainPerson>> GetPagedAsync(PersonSortBy sortBy, int page, int pageSize, IEnumerable<PersonStatus> statuses, string? search, IEnumerable<string> tags, Guid? userId, CancellationToken cancellationToken);
    Task<DomainPerson> GetByIdAsync(Guid personId, CancellationToken cancellationToken);
    Task AddAsync(DomainPerson person, CancellationToken cancellationToken);
}