namespace RespectCounter.API.Requests;

public record ProposeActivityRequest(
    int Type, 
    string PersonId,
    string Title, 
    string? Description, 
    string? Location, 
    string? Happend, 
    string Source, 
    IEnumerable<string> Tags
);