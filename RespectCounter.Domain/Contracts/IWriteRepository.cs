using System.Linq.Expressions;
using RespectCounter.Domain.Model;

namespace RespectCounter.Domain.Contracts;

public interface IWriteRepository
{
    T Add<T>(T entity) where T : Auditable;
    void Update<T>(T entity) where T : Auditable;
    Task SoftDeleteByIdAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Entity;
}