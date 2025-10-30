using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Shared.DTOs;

public record PersonDTO(
    string Id,
    string FirstName,
    string LastName,
    string NickName,
    string FullName,
    string Profession,
    string Description,
    string AvatarUrl,
    string Nationality,
    string Birthday,
    string DeathDate,
    string Status,
    string CreatedBy,
    string CreatedById,
    List<SimpleTagDTO> Tags,
    int ActivitiesCount,
    int CommentsCount,
    int Respect,
    ReactionType? CurrentUsersReaction
);