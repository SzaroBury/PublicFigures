using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Services;

public class EntityChecker : IEntityChecker
{
    private readonly IReadOnlyRepository _repository;

    public EntityChecker(IReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExistsAsync<T>(Guid entityId, CancellationToken cancellationToken) where T : Entity
    {
        return await _repository
            .FindQueryable<T>(entity => entity.Id == entityId)
            .AnyAsync(cancellationToken);
    }
}