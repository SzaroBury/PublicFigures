using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Shared.DTOs;

public record ActivityDTO(
    string Id,
    string PersonId,
    string PersonFullName,
    int PersonRespect,
    string PersonImagePath,
    string Status,
    string CreatedBy,
    string CreatedById,
    string Value,
    string Description, 
    string Location, 
    string Source, 
    string Tags,
    string OccurredAt, 
    int CommentsCount,
    int Type, 
    int Respect,
    ReactionType? CurrentUsersReaction
);