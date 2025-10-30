using RespectCounter.API.Requests;
using RespectCounter.Application.Activity.Commands;
using RespectCounter.Domain.Enums;

namespace RespectCounter.API.Mappers;

public static class ActivityMappingExtensions
{
    public static AddActivityCommand ToAddCommand(this ProposeActivityRequest request, string userId)
    {
        return new AddActivityCommand(
            request.PersonId,
            request.Title,
            request.Description ?? "",
            request.Location ?? "",
            request.Happend ?? "",
            request.Source,
            request.Type,
            request.Tags,
            userId
        );
    }

    public static UpdateActivityCommand ToUpdateCommand(this ProposeActivityRequest request, string activityId, string userId)
    {
        return new(
            activityId,
            request.PersonId,
            request.Title,
            request.Description ?? "",
            request.Location ?? "",
            request.Happend ?? "",
            request.Source,
            request.Type,
            request.Tags,
            userId);
    }

    public static HashSet<ActivityStatus> ToActivityStatusHashSet(this bool? onlyVerified)
    {
        var result = new HashSet<ActivityStatus>();
        if (onlyVerified.HasValue && onlyVerified.Value)
        {
            result = [ActivityStatus.Verified];
        }
        return result;
    }
}