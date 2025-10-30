using RespectCounter.API.Requests;
using RespectCounter.Application.Person.Commands;

namespace RespectCounter.API.Mappers;

public static class PersonMappingExtensions
{
    public static AddPersonCommand ToAddCommand(this ProposePersonRequest request, string userId)
    {
        return new AddPersonCommand(
            request.FirstName,
            request.LastName,
            request.NickName ?? "",
            request.Profession,
            request.Description ?? "",
            request.Nationality,
            request.Birthday,
            request.DeathDate,
            request.Tags,
            userId
        );
    }

    public static UpdatePersonCommand ToUpdateCommand(this ProposePersonRequest request, string personId, string userId)
    {
        return new UpdatePersonCommand(
            personId,
            request.FirstName,
            request.LastName,
            request.NickName ?? "",
            request.Profession,
            request.Description ?? "",
            request.Nationality,
            request.Birthday,
            request.DeathDate,
            request.Tags,
            userId
        );
    }
}