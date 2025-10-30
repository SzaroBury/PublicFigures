using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainActivity = RespectCounter.Domain.Model.Activity;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Tags.Commands
{
    public record AddTagToActivityCommand(
        string ActivityId,
        string TagName,
        string UserId
    ) : IRequest<ActivityDTO>;

    public class AddTagToActivityCommandHandler : IRequestHandler<AddTagToActivityCommand, ActivityDTO>
    {
        private readonly IReadOnlyRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IIdentityService _userService;

        public AddTagToActivityCommandHandler(IReadOnlyRepository repository, IUnitOfWork uow, IIdentityService userService)
        {
            _repository = repository;
            _uow = uow;
            _userService = userService;
        }

        public async Task<ActivityDTO> Handle(AddTagToActivityCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId.ToGuid();
            var user = await _repository.FindByIdAsync<User>(userId, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

            var activityId = request.ActivityId.ToGuid();
            DomainActivity targetActivity = await _repository.SingleOrDefaultAsync<DomainActivity>(
                a => a.Id == activityId,
                "Tags",
                cancellationToken)
                ?? throw new InvalidOperationException($"The Activity with ID {request.ActivityId} was not found in the system, despite the previous validation check.");
                
            Tag? existingTag = _repository.FindQueryable<Tag>(
                t => t.Name.Equals(request.TagName, StringComparison.CurrentCultureIgnoreCase)
            ).FirstOrDefault();

            DateTime now = DateTime.UtcNow;
            if (existingTag == null)
            {
                Tag newTag = new(user, now)
                {
                    Name = request.TagName,
                    Description = $"Created for {targetActivity.Id} activity object."
                };
                existingTag = _uow.GetWriteRepository().Add(newTag);
            }
            else if (targetActivity.Tags.Any(at => at.Tag.Name.ToLower() == existingTag.Name.ToLower()))
            {
                throw new InvalidOperationException("The pointed activity already has the given tag.");
            }
            ActivityTag activityTag = new(targetActivity, existingTag, user, now);
            targetActivity.Tags.Add(activityTag);

            await _uow.CommitAsync(cancellationToken);
            return targetActivity.ToDTO(userId);
        }
    }
}