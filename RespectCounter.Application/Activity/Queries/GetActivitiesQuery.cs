using MediatR;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared;

namespace RespectCounter.Application.Activity.Queries;

public record GetActivitiesQuery(
    string Search, 
    string? PersonId,
    string? Type,
    IEnumerable<string>? Tags, 
    HashSet<ActivityStatus>? Status,
    string? Order,
    int Page,
    int PageSize,
    string? UserId,
    string? CurrentUserId
) : IRequest<PagedResult<ActivityDTO>>;

public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, PagedResult<ActivityDTO>>
{
    private readonly IActivityRepository _repository;

    public GetActivitiesQueryHandler(IActivityRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<ActivityDTO>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
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

        var sortBy = request.Order.ToActivitySortByEnum();

        ActivityType? activityType = null;
        if(!string.IsNullOrEmpty(request.Type))
        {
            activityType = request.Type.ToActivityTypeEnum();
        }

        var result = await _repository.FindPagedResultAsync(
            request.Page, 
            request.PageSize, 
            sortBy, 
            statusFilter, 
            request.Type?.ToActivityTypeEnum(),
            request.Search.Trim().ToLower(),
            request.Tags?.Select(t => t.Trim().ToLower()),
            request.PersonId.ToNullableGuid(),
            request.UserId.ToNullableGuid(),
            cancellationToken
        );

        return new PagedResult<ActivityDTO>()
        {
            Items = result.Items.Select(i => i.ToDTO()),
            TotalItems = result.TotalItems,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}