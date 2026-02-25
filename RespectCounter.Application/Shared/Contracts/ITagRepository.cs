using DomainTag = RespectCounter.Domain.Model.Tag;

namespace RespectCounter.Application.Shared.Contracts;

public interface ITagRepository
{
    // Task<DomainTag> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainTag>> GetByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainTag>> GetPersonTagsAsync(Guid personId, int AtLeastCount = 1, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainTag>> GetAllNonEmptyTagsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainTag>> GetAllAsync(int atLeastCount = 1, CancellationToken cancellationToken = default);
    Task<DomainTag> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task AddTagAsync(DomainTag tag, CancellationToken cancellationToken = default);
}