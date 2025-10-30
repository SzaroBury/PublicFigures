using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainActivity = RespectCounter.Domain.Model.Activity;

namespace RespectCounter.Application.Activity.Commands
{
    public record HideActivityCommand(string ActivityId, string UserId) : IRequest<ActivityDTO>;

    public class HideActivityCommandHandler : IRequestHandler<HideActivityCommand, ActivityDTO>
    {
        private readonly IReadOnlyRepository _repository;
        private readonly IUnitOfWork _uow;

        public HideActivityCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<ActivityDTO> Handle(HideActivityCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;

            Guid userId = request.UserId.ToGuid();
            var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

            Guid activityGuid = request.ActivityId.ToGuid();
            var activity = await _repository.FindByIdAsync<DomainActivity>(activityGuid)
                ?? throw new InvalidOperationException($"The Activity with ID {request.ActivityId} was not found in the system, despite the previous validation check.");

            activity.Hide(user, now);
            await _uow.CommitAsync(cancellationToken);

            return activity.ToDTO();
        }
    }
}