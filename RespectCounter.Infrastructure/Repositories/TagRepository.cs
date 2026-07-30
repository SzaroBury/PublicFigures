using Microsoft.EntityFrameworkCore;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Repositories;

public class TagRepository: ITagRepository
{
    private readonly RespectDbContext _dbContext;

    public TagRepository(RespectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Tag>> GetAllNonEmptyTagsWithoutTrackingAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tags
            .AsNoTracking()
            .Include(t => t.Activities)
            .Include(t => t.Persons)
            .Where(t => t.Count > 0)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Tag>> GetPersonTagsWithoutTrackingAsync(
        Guid personId, 
        int atLeastCount = 1, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.PersonTag
            .AsNoTracking()
            .Include(pt => pt.Tag).ThenInclude(t => t.Activities)
            .Include(pt => pt.Tag).ThenInclude(t => t.Persons)
            .Where(pt => pt.PersonId == personId && pt.Tag.Count >= atLeastCount)
            .Select(pt => pt.Tag)
            .ToListAsync(cancellationToken);
    }


    public async Task<IEnumerable<Tag>> GetAllWithoutTrackingAsync(int atLeastCount = 0, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tags
            .AsNoTracking()
            .Include(t => t.Activities)
            .Include(t => t.Persons)
            .Where(t => t.Count >= atLeastCount)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Tag>> GetByNamesWithoutTrackingAsync(IEnumerable<string> names, CancellationToken cancellationToken = default)
    {
        var normalizedNames = names.Select(t => t.ToLowerInvariant());

        return await _dbContext.Tags
            .AsNoTracking()
            .Include(t => t.Activities)
            .Include(t => t.Persons)
            .Where(t => normalizedNames.Contains(t.Name))
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Tag> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tags
            .Include(t => t.Activities)
            .Include(t => t.Persons)
            .Where(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            .OrderBy(t => t.Name)
            .FirstAsync(cancellationToken);
    }

    public void AddTag(Tag tag)
    {
        _dbContext.Tags.Add(tag);
    }

}