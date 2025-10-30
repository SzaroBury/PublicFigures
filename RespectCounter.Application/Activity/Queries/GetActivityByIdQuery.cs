using MediatR;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainActivity = RespectCounter.Domain.Model.Activity;

namespace RespectCounter.Application.Activity.Queries;

public record GetActivityByIdQuery(string ActivityId, string? UserId) : IRequest<ActivityDTO>;

public class GetActivityByIdQueryHandler : IRequestHandler<GetActivityByIdQuery, ActivityDTO>
{
    private readonly IReadOnlyRepository _repository;

    public GetActivityByIdQueryHandler(IReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<ActivityDTO> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = null;
        if(!string.IsNullOrEmpty(request.UserId))
        {
            userId = request.UserId.ToGuid();
        }

        var actGuid = request.ActivityId.ToGuid();
        var act = await _repository.SingleOrDefaultAsync<DomainActivity>(
                a => a.Id == actGuid,
                "Person,Comments.Children,Reactions,Tags,CreatedBy,LastUpdatedBy",
                cancellationToken
            ) ?? throw new KeyNotFoundException("The activity was not found. Please enter Id of an existing activity.");

        return act.ToDTO(userId);
    }
}