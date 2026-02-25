using MediatR;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Activity.Queries;

public record GetActivityByIdQuery(string ActivityId, string? UserId) : IRequest<ActivityDTO>;

public class GetActivityByIdQueryHandler : IRequestHandler<GetActivityByIdQuery, ActivityDTO>
{
    private readonly IActivityRepository _activityRepository;

    public GetActivityByIdQueryHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<ActivityDTO> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = null;
        if(!string.IsNullOrEmpty(request.UserId))
        {
            userId = request.UserId.ToGuid();
        }

        var actGuid = request.ActivityId.ToGuid();
        var act = await _activityRepository.FindByIdAsync(actGuid, cancellationToken)
            ?? throw new KeyNotFoundException("The activity was not found. Please enter Id of an existing activity.");

        return act.ToDTO(userId);
    }
}