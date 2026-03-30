using Microsoft.EntityFrameworkCore;
using RespectCounter.Application.Shared;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Enums;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Enums;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Repositories;

public class CommentRepository: ICommentRepository
{
    private readonly RespectDbContext _dbContext;

    public CommentRepository(RespectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Comment> GetCommentByIdAsync(Guid guid, CancellationToken cancellationToken)
    {
        Comment? comment = await _dbContext.Comment
            .Include(c => c.Children)
            .Include(c => c.Parent)
            .Include(c => c.Reactions)
            .Include(c => c.CreatedBy)
            .Include(c => c.LastUpdatedBy)
            .FirstOrDefaultAsync(c => c.Id == guid, cancellationToken);
            
        
        if(comment == null)
        {
            throw new KeyNotFoundException($"The comment with ID '{guid}' was not found.");
        }
        
        return comment;
    }

    public async Task<PagedResult<Comment>> GetPagedCommentsForActivityAsync(Guid activityId, CommentSortBy sortBy, IReadOnlyCollection<CommentStatus> statuses, int page, int pageSize, CancellationToken cancellationToken)
    {
        List<Comment> comments = await _dbContext.Comment
            .AsNoTracking()
            .Where(c => statuses.Contains(c.Status) && c.ActivityId == activityId)
            .Include(c => c.Children)
            .Include(c => c.Reactions)
            .Include(c => c.CreatedBy)
            .Include(c => c.LastUpdatedBy)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ApplySorting(sortBy)
            .ToListAsync(cancellationToken);

        return new PagedResult<Comment>
        {
            Items = comments,
            TotalItems = comments.Count(),
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public async Task<PagedResult<Comment>> GetPagedCommentsForPersonAsync(Guid personId, CommentSortBy sortBy, IReadOnlyCollection<CommentStatus> statuses, int page, int pageSize, CancellationToken cancellationToken)
    {
        List<Comment> comments = await _dbContext.Comment
            .AsNoTracking()
            .Where(c => statuses.Contains(c.Status) && c.PersonId == personId)
            .Include(c => c.Children)
            .Include(c => c.Reactions)
            .Include(c => c.CreatedBy)
            .Include(c => c.LastUpdatedBy)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ApplySorting(sortBy)
            .ToListAsync(cancellationToken);

        return new PagedResult<Comment>
        {
            Items = comments,
            TotalItems = comments.Count(),
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public void AddComment(Comment comment)
    {
        _dbContext.Comment.Add(comment);
    }

    public async Task UpdateAncestorsCountsDirectAsync(Guid parentGuid, CancellationToken cancellationToken)
    {
        await _dbContext.Comment.ExecuteUpdateAsync(s => s
                .SetProperty(c => c.AllChildrenCount, c => c.AllChildrenCount + 1)
                .SetProperty(c => c.DirectChildrenCount, c => c.DirectChildrenCount + 1), 
            cancellationToken);
        Guid? nextParent = await _dbContext.Comment.Where(c => c.Id == parentGuid).Select(c => c.Id).FirstAsync(cancellationToken);

        while(nextParent.HasValue)
        {
            await _dbContext.Comment.ExecuteUpdateAsync(s => s.SetProperty(c => c.AllChildrenCount, c => c.AllChildrenCount + 1), cancellationToken);
            nextParent = await _dbContext.Comment.Where(c => c.Id == nextParent).Select(c => c.Id).FirstAsync(cancellationToken);
        }
    }
}