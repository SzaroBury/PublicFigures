namespace RespectCounter.Application.Shared.DTOs;

public record IdentityDTO(
    Guid Id,
    string Username,
    IList<string> Roles
);