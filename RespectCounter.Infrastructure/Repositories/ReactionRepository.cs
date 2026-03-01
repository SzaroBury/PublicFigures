using Microsoft.EntityFrameworkCore;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Repositories;

public class ReactionRepository : IReactionRepository
{
    private readonly RespectDbContext _dbContext;

    public ReactionRepository(RespectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<ActivityReaction?> TryGetCurrentUserReactionForActivityAsync(Guid activityId, Guid userId, CancellationToken cancellationToken = default)
    {
        return _dbContext.ActivityReactions
            .Where(ar => ar.ActivityId == activityId && ar.CreatedById == userId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<CommentReaction?> TryGetCurrentUserReactionForCommentAsync(Guid commentId, Guid userId, CancellationToken cancellationToken = default)
    {
        return _dbContext.CommentReactions
            .Where(cr => cr.CommentId == commentId && cr.CreatedById == userId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<PersonReaction?> TryGetCurrentUserReactionForPersonAsync(Guid personId, Guid userId, CancellationToken cancellationToken = default)
    {
        return _dbContext.PersonReactions
            .Where(pr => pr.PersonId == personId && pr.CreatedById == userId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}