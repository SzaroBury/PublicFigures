using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.Enums;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared;

namespace RespectCounter.Infrastructure.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly RespectDbContext _dbContext;

    public PersonRepository(RespectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Person>> GetPagedAsync(PersonSortBy sortBy, int page, int pageSize, IEnumerable<PersonStatus> statuses, string? search, IEnumerable<string> tags, Guid? userId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Persons.AsNoTracking().Where(
            a => statuses.Contains(a.Status)
        );

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(p =>
                (p.FirstName != null && p.FirstName.ToLower().Contains(search)) ||
                (p.LastName != null && p.LastName.ToLower().Contains(search)) ||
                (p.NickName != null && p.NickName.ToLower().Contains(search)) ||
                (p.Profession != null && p.Profession.ToLower().Contains(search)) ||
                // (p.Description != null && p.Description.ToLower().Contains(search)) ||
                (p.Nationality != null && p.Nationality.ToLower().Contains(search)) ||
                p.Tags.Any(at => at.Tag.Name != null && at.Tag.Name.ToLower().Contains(search))
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


        if (userId.HasValue)
        {
            query = query.Where(a => a.CreatedById == userId);
        }

        var totalRecords = await query.CountAsync(cancellationToken); 
        if(totalRecords == 0)
        {
            return new PagedResult<Person>()
            {
                Items = Enumerable.Empty<Person>(),
                TotalItems = 0,
                PageNumber = page,
                PageSize = pageSize,
            };
        }

        query = query.ApplySorting(sortBy);
        
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(a => a.Comments).ThenInclude(c => c.Children)
            .Include(a => a.Reactions)
            .Include(a => a.Tags).ThenInclude(at => at.Tag)
            .Include(a => a.CreatedBy)
            .Include(a => a.LastUpdatedBy)
            .ToListAsync(cancellationToken);

        return new PagedResult<Person>()
        {
            Items = items,
            TotalItems = totalRecords,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public async Task<Person> GetByIdAsync(Guid personId, CancellationToken cancellationToken)
    {
        Person? person = await _dbContext.Persons
            .Include(p => p.Activities)
            .Include(p => p.Comments).ThenInclude(c => c.Children)
            .Include(p => p.Reactions)
            .Include(p => p.Tags).ThenInclude(pt => pt.Tag)
            .Include(p => p.CreatedBy)
            .Include(p => p.LastUpdatedBy)
            .FirstOrDefaultAsync(p => p.Id == personId);
        
        if(person == null)
        {
            throw new KeyNotFoundException($"The person with ID '{personId}' was not found.");
        }
        
        return person;
    }

    public async Task AddAsync(Person person, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(person);
    }
}