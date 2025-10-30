using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainActivity = RespectCounter.Domain.Model.Activity;

namespace RespectCounter.Application.Activity.Commands
{
    public record VerifyActivityCommand(string ActivityId, string UserId) : IRequest<ActivityDTO>;

    public class VerifyActivityCommandHandler : IRequestHandler<VerifyActivityCommand, ActivityDTO>
    {
        private readonly IReadOnlyRepository _repository;
        private readonly IUnitOfWork _uow;

        public VerifyActivityCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<ActivityDTO> Handle(VerifyActivityCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;

            Guid userId = request.UserId.ToGuid();
            var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

            Guid activityGuid = request.ActivityId.ToGuid();
            var activity = await _repository.FindByIdAsync<DomainActivity>(activityGuid)
                ?? throw new KeyNotFoundException($"The activity with the given ID ({request.ActivityId}) was not found.");

            activity.Verify(user, now);
            await _uow.CommitAsync(cancellationToken);

            return activity.ToDTO();
        }
    }
}