using Microsoft.EntityFrameworkCore.Storage;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Infrastructure.Repositories;

/// <summary>
/// Unit of work: Maintains a list of objects affected by a business transaction and coordinates the writing out of changes and the resolution of concurrency problems. https://www.martinfowler.com/eaaCatalog/unitOfWork.html
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly RespectDbContext context;
    private bool disposed;
    public UnitOfWork(RespectDbContext context)
    {
        this.context = context;
        context.Database.EnsureCreated();
    }

    public IWriteRepository GetWriteRepository()
    {
        return new WriteRepository(context);
    }

    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        disposed = true;
    }
}