using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Activity.Commands
{
    public record HideActivityCommand(string ActivityId, string UserId) : IRequest<ActivityDTO>;

    public class HideActivityCommandHandler : IRequestHandler<HideActivityCommand, ActivityDTO>
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IUnitOfWork _uow;

        public HideActivityCommandHandler(IReadOnlyRepository readOnlyRepository, IActivityRepository activityRepository, IUnitOfWork uow)
        {
            _readOnlyRepository = readOnlyRepository;
            _activityRepository = activityRepository;
            _uow = uow;
        }

        public async Task<ActivityDTO> Handle(HideActivityCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;

            Guid userId = request.UserId.ToGuid();
            var user = await _readOnlyRepository.FindByIdAsync<User>(userId, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

            Guid activityGuid = request.ActivityId.ToGuid();
            var activity = await _activityRepository.FindByIdAsync(activityGuid)
                ?? throw new InvalidOperationException($"The Activity with ID {request.ActivityId} was not found in the system, despite the previous validation check.");

            activity.Hide(user, now);
            await _uow.CommitAsync(cancellationToken);

            return activity.ToDTO();
        }
    }
}