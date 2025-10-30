using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainActivity = RespectCounter.Domain.Model.Activity;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Activity.Queries;

public record GetActivitiesQuery(
    string Search, 
    string? PersonId,
    string? Type,
    string Tags, 
    HashSet<ActivityStatus>? Status,
    string? Order,
    int Page,
    int PageSize,
    string? UserId
) : IRequest<IEnumerable<ActivityDTO>>;

public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, IEnumerable<ActivityDTO>>
{
    private readonly IReadOnlyRepository _repository;

    public GetActivitiesQueryHandler(IReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ActivityDTO>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ActivityStatus> statusFilter;
        if (request.Status == null || request.Status.Count == 0)
        {
            statusFilter = [ActivityStatus.Verified, ActivityStatus.NotVerified];
        }
        else
        {
            statusFilter = request.Status!;
        }

        var query = _repository.FindQueryable<DomainActivity>(
            a => statusFilter.Contains(a.Status)
        );

        if (!string.IsNullOrEmpty(request.PersonId))
        {
            var personGuid = request.PersonId.ToGuid();
            query.Where(a => a.PersonId == personGuid);
        }

        if(!string.IsNullOrEmpty(request.Type))
        {
            var activityType = request.Type.ToActivityTypeEnum();
            query = query.Where(a => a.Type == activityType);
        }

        if (!string.IsNullOrEmpty(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(a =>
                (a.Value != null && a.Value.ToLower().Contains(search)) ||
                (a.Source != null && a.Source.ToLower().Contains(search)) ||
                (a.Description != null && a.Description.ToLower().Contains(search)) ||
                a.Tags.Any(at => at.Tag.Name != null && at.Tag.Name.ToLower().Contains(search))
            );
        }
        
        if(!string.IsNullOrEmpty(request.Tags))
        {
            var tags = request.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim().ToLower());
            query = query.Where(
                a => tags.All(
                    tag => a.Tags.Any(
                        at => at.Tag.Name.Equals(tag, StringComparison.CurrentCultureIgnoreCase)
                    )
                )
            );
        }

        var sortBy = request.Order.ToActivitySortByEnum();
        var orderedQuery = query.ApplySorting(sortBy);

        orderedQuery = orderedQuery.ApplyPaging(request.Page, request.PageSize);

        var activities = await _repository
            .FindListAsync(
                orderedQuery,
                ["Person", "Comments.Children", "Reactions", "Tags", "CreatedBy", "LastUpdatedBy"],
                null,
                cancellationToken
            );

        Guid? userId = null;
        if(!string.IsNullOrEmpty(request.UserId))
        {
            userId = Guid.Parse(request.UserId);
            var user = await _repository.FindByIdAsync<User>(userId.Value, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");
            userId = user?.Id;
        }

        return activities.Select(a => a.ToDTO(userId));
    }
}