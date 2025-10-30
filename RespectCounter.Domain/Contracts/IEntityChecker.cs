using RespectCounter.Domain.Model;

namespace RespectCounter.Domain.Contracts;

public interface IEntityChecker
{
    Task<bool> ExistsAsync<T>(Guid id, CancellationToken cancellationToken) where T : Entity;
}