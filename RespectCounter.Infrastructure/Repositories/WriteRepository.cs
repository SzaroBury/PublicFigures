using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Infrastructure.Repositories;

public class WriteRepository : IWriteRepository
{
    private readonly RespectDbContext dbContext;

    public WriteRepository(RespectDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public T Add<T>(T entity) where T : Auditable
    {
        return dbContext.Set<T>().Add(entity).Entity;
    }

    public void Update<T>(T entity) where T : Auditable
    {
        dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task SoftDeleteByIdAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Entity
    {
        var item = await dbContext.Set<T>().FindAsync([id], cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException($"The item with the given ID ({id}) was not found.");

        throw new NotImplementedException();
        // item.Deleted = true;
        //item.DeletedDate = DateTime.UtcNow;
        // dbContext.Update(item);
    }
}