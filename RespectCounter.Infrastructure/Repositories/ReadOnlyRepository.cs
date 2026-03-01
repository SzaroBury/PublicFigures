using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Infrastructure.Repositories;

public class ReadOnlyRepository : IReadOnlyRepository
{
    private readonly RespectDbContext dbContext;

    public ReadOnlyRepository(RespectDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<T?> FindByIdAsync<T>(
        Guid id,
        CancellationToken cancellationToken = default
    ) where T : Entity
    {
        return dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task<bool> ExistsAsync<T>(Guid entityId, CancellationToken cancellationToken) where T : Entity
    {
        return dbContext.Set<T>().AsNoTracking().AnyAsync(entity => entity.Id == entityId);
    }

    public Task<List<T>> FindListAsync<T>(
        CancellationToken cancellationToken = default
    ) where T : class
    {
        return dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }
}