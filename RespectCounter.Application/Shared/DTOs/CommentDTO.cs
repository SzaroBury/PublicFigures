using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Shared.DTOs;

public record CommentDTO(
    string Id, 
    string CreatedBy,
    string CreatedById,
    string AvatarUrl,
    string Created,
    string Content, 
    string ActivityId,
    string PersonId,
    string ParentId,
    int Status,
    int Respect,
    int ChildrenCount,
    List<CommentDTO> Children,
    ReactionType? CurrentUsersReaction
);