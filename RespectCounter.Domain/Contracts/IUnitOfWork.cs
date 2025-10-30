namespace RespectCounter.Domain.Contracts;

public interface IUnitOfWork : IDisposable
{
    IWriteRepository GetWriteRepository();
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}