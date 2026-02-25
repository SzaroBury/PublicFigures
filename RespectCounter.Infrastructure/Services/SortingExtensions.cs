using RespectCounter.Application.Shared.Enums;
using DomainActivity = RespectCounter.Domain.Model.Activity;
using DomainComment = RespectCounter.Domain.Model.Comment;
using DomainPerson = RespectCounter.Domain.Model.Person;

namespace RespectCounter.Application.Shared.Extensions;

public static class SortingExtensions
{

    public static IQueryable<DomainActivity> ApplySorting(this IQueryable<DomainActivity> query, ActivitySortBy sortBy, int trendingDays = 7)
    {
        return sortBy switch
        {
            ActivitySortBy.LatestAdded => query.OrderByDescending(static a => a.Created),
            ActivitySortBy.LatestHappend => query.OrderByDescending(static a => a.Happend),
            ActivitySortBy.BestMatching => throw new NotImplementedException(),
            ActivitySortBy.MostLiked => query.OrderByDescending(
                a => a.Reactions.Sum(r => (int)r.ReactionType)),
            ActivitySortBy.LeastLiked => query.OrderBy(
                a => a.Reactions.Sum(r => (int)r.ReactionType)),
            ActivitySortBy.Trending => query.OrderByDescending(
                a => a.Reactions.Where(r => r.Created > DateTime.UtcNow.AddDays(-trendingDays))
                    .Sum(r => (int)r.ReactionType)),
            _ => throw new NotImplementedException()
        };
        throw new NotImplementedException();

    }

    public static IQueryable<DomainPerson> ApplySorting(this IQueryable<DomainPerson> query, PersonSortBy sortBy, int trendingDays = 7)
    {
        return sortBy switch
        {
            PersonSortBy.Trending => query.OrderByDescending(
                p => p.Reactions.Where(r => r.Created > DateTime.UtcNow.AddDays(-trendingDays))
                    .Sum(r => (int)r.ReactionType)),
            PersonSortBy.MostRespected => query.OrderByDescending(
                p => p.Reactions.Sum(r => (int)r.ReactionType)),
            PersonSortBy.LeastRespected => query.OrderBy(
                p => p.Reactions.Sum(r => (int)r.ReactionType)),
            PersonSortBy.LatestAdded => query.OrderByDescending(p => p.Created),
            PersonSortBy.AlphabeticalLastname => query.OrderBy(p => p.LastName),
            PersonSortBy.AlphabeticalReversedLastname => query.OrderByDescending(p => p.LastName),
            PersonSortBy.BestMatching => throw new NotImplementedException(),
            _ => query
        };
    }

    public static IQueryable<DomainComment> ApplySorting(this IQueryable<DomainComment> query, CommentSortBy sortBy)
    {
        return sortBy switch
        {
            CommentSortBy.MostRespected => query.OrderByDescending(
                p => p.Reactions.Sum(r => (int)r.ReactionType)),
            CommentSortBy.LeastRespected => query.OrderBy(
                p => p.Reactions.Sum(r => (int)r.ReactionType)),
            CommentSortBy.LatestAdded => query.OrderByDescending(p => p.Created),
            CommentSortBy.OldestAdded => query.OrderBy(p => p.Created),
            _ => query
        };
    }
}