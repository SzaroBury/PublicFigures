using RespectCounter.Domain.Model;

namespace RespectCounter.Application.Shared.Contracts;

public interface IReadOnlyRepository
{
    Task<bool> ExistsAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Entity;
    Task<T?> FindByIdAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Entity;
    Task<List<T>> FindListAsync<T>(CancellationToken cancellationToken = default) where T : class;
}