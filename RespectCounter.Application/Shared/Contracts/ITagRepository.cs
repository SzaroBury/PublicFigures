using DomainTag = RespectCounter.Domain.Model.Tag;

namespace RespectCounter.Application.Shared.Contracts;

public interface ITagRepository
{
    // Task<DomainTag> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainTag>> GetAllNonEmptyTagsWithoutTrackingAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainTag>> GetPersonTagsWithoutTrackingAsync(Guid personId, int AtLeastCount = 1, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainTag>> GetAllWithoutTrackingAsync(int atLeastCount = 1, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainTag>> GetByNamesWithoutTrackingAsync(IEnumerable<string> names, CancellationToken cancellationToken = default);
    Task<DomainTag> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    void AddTag(DomainTag tag);
}