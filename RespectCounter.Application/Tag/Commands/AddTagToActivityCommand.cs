using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.DTOs;
using RespectCounter.Application.Shared.Extensions;
using DomainActivity = RespectCounter.Domain.Model.Activity;
using DomainTag = RespectCounter.Domain.Model.Tag;

namespace RespectCounter.Application.Tag.Commands
{
    public record AddTagToActivityCommand(
        string ActivityId,
        string TagName,
        string UserId
    ) : IRequest<ActivityDTO>;

    public class AddTagToActivityCommandHandler : IRequestHandler<AddTagToActivityCommand, ActivityDTO>
    {
        private readonly IReadOnlyRepository _roRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _uow;

        public AddTagToActivityCommandHandler(
            IReadOnlyRepository roRepository, 
            IActivityRepository activityRepository,
            ITagRepository tagRepository,
            IUnitOfWork uow)
        {
            _roRepository = roRepository;
            _activityRepository = activityRepository;
            _tagRepository = tagRepository;
            _uow = uow;
        }

        public async Task<ActivityDTO> Handle(AddTagToActivityCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId.ToGuid();
            var user = await _roRepository.FindByIdAsync<User>(userId, cancellationToken)
                ?? throw new InvalidOperationException($"The User with ID {userId} was not found in the system, despite the previous validation check.");

            var activityId = request.ActivityId.ToGuid();
            DomainActivity targetActivity = await _activityRepository.FindByIdAsync(activityId, cancellationToken)
                ?? throw new InvalidOperationException($"The Activity with ID {request.ActivityId} was not found in the system, despite the previous validation check.");
                
            DateTime now = DateTime.UtcNow;
            DomainTag? tag = await _tagRepository.GetByNameAsync(request.TagName.ToLower(), cancellationToken);
            if (tag == null)
            {
                tag = new(user, now)
                {
                    Name = request.TagName,
                    Description = $"Created for '{targetActivity.Id}' activity object."
                };
                _tagRepository.AddTag(tag);
            }
            else if (targetActivity.Tags.Any(at => at.Tag.Name.ToLower() == tag.Name.ToLower()))
            {
                throw new InvalidOperationException("The pointed activity already has the given tag.");
            }
            ActivityTag activityTag = new(targetActivity, tag, user, now);
            targetActivity.Tags.Add(activityTag);

            await _uow.CommitAsync(cancellationToken);
            return targetActivity.ToDTO(userId);
        }
    }
}