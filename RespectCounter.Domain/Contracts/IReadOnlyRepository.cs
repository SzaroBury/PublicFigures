using System.Linq.Expressions;
using RespectCounter.Domain.Model;

namespace RespectCounter.Domain.Contracts;

public interface IReadOnlyRepository
{
    Task<T?> FindByIdAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Entity;
    IQueryable<T> FindQueryable<T>(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where T : Entity;
    Task<List<T>> FindListAsync<T>(Expression<Func<T, bool>>? filter, string[]? includeProperties = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default) where T : class;
    Task<List<T>> FindListAsync<T>(IQueryable<T> query, string[]? includeProperties = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default) where T : class;
    Task<T?> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> filter, string? includeProperties = null, CancellationToken cancellationToken = default) where T : Entity;
}