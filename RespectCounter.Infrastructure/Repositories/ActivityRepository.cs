using Microsoft.EntityFrameworkCore;
using RespectCounter.Application.Shared;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Enums;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly RespectDbContext _dbContext;

    public ActivityRepository(RespectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Activity>> FindPagedResultAsync(
        int page,
        int pageSize,
        ActivitySortBy sortBy,
        IEnumerable<ActivityStatus> statuses,
        ActivityType? type = null,
        string? search = null,
        IEnumerable<string>? tags = null,
        Guid? personId = null,
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Activities.AsNoTracking().Where(
            a => statuses.Contains(a.Status)
        );

        if(type.HasValue)
        {
            query = query.Where(a => a.Type == type);
        }

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(a =>
                (a.Value != null && a.Value.ToLower().Contains(search)) ||
                (a.Source != null && a.Source.ToLower().Contains(search)) ||
                (a.Description != null && a.Description.ToLower().Contains(search)) ||
                a.Tags.Any(at => at.Tag.Name != null && at.Tag.Name.ToLower().Contains(search))
            );
        }
        
        if(tags != null && tags.Any())
        {
            query = query.Where(
                a => tags.All(
                    tag => a.Tags.Any(
                        at => at.Tag.Name.ToLower() == tag
                    )
                )
            );
        }

        if (personId.HasValue)
        {
            query = query.Where(a => a.PersonId == personId);
        }


        if (userId.HasValue)
        {
            query = query.Where(a => a.CreatedById == userId);
        }

        var totalRecords = await query.CountAsync(cancellationToken); 
        if(totalRecords == 0)
        {
            return new PagedResult<Activity>()
            {
                Items = Enumerable.Empty<Activity>(),
                TotalItems = 0,
                PageNumber = page,
                PageSize = pageSize,
            };
        }

        query = query.ApplySorting(sortBy);
        
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(a => a.Person)
            .Include(a => a.Comments).ThenInclude(c => c.Children)
            .Include(a => a.Reactions)
            .Include(a => a.Tags).ThenInclude(at => at.Tag)
            .Include(a => a.CreatedBy)
            .Include(a => a.LastUpdatedBy)
            .ToListAsync(cancellationToken);

        return new PagedResult<Activity>()
        {
            Items = items,
            TotalItems = totalRecords,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public Task<Activity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Activities
            .Include(a => a.Person)
            .Include(a => a.Comments).ThenInclude(c => c.Children)
            .Include(a => a.Reactions)
            .Include(a => a.Tags).ThenInclude(at => at.Tag)
            .Include(a => a.CreatedBy)
            .Include(a => a.LastUpdatedBy)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public void Add(Activity activity)
    {
        _dbContext.Activities.Add(activity);
    }

    public void Update(Activity activity)
    {
        _dbContext.Activities.Update(activity);
    }
}