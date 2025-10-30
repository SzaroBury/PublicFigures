using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Infrastructure.Repositories;

public class ReadOnlyRepository : IReadOnlyRepository
{
    private readonly RespectDbContext dbContext;

    public ReadOnlyRepository(RespectDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<T?> FindByIdAsync<T>(
        Guid id,
        CancellationToken cancellationToken = default
    ) where T : Entity
    {
        return await dbContext.Set<T>().FindAsync([id], cancellationToken: cancellationToken);
    }

    public IQueryable<T> FindQueryable<T>(
        Expression<Func<T, bool>> filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
    ) where T : Entity
    {
        var query = dbContext.Set<T>().Where(filter);
        return orderBy != null ? orderBy(query) : query;
    }

    public Task<List<T>> FindListAsync<T>(
        Expression<Func<T, bool>>? filter,
        string[]? includeProperties = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        IQueryable<T> query;

        if (filter == null)
        {
            query = dbContext.Set<T>();
        }
        else
        {
            query = dbContext.Set<T>().Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (string prop in includeProperties)
            {
                query = query.Include(prop);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return  query.ToListAsync(cancellationToken);
    }

    public Task<List<T>> FindListAsync<T>(
        IQueryable<T> query,
        string[]? includeProperties = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        if (includeProperties != null)
        {
            foreach (string prop in includeProperties)
            {
                query = query.Include(prop);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return query.ToListAsync(cancellationToken);
    }

    public Task<T?> SingleOrDefaultAsync<T>(
        Expression<Func<T, bool>> filter,
        string? includeProperties = null,
        CancellationToken cancellationToken = default
    ) where T : Entity
    {
        var query = dbContext.Set<T>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            query = includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        return query.SingleOrDefaultAsync(filter, cancellationToken);
    }
}