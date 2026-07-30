namespace RespectCounter.API.Requests;

public record ProposeActivityRequest(
    int Type, 
    string PersonId,
    string Title, 
    string? Description, 
    string? Location, 
    string? OccurredAt, 
    string Source, 
    IEnumerable<string> Tags
);