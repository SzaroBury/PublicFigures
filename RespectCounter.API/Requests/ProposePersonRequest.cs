namespace RespectCounter.API.Requests;

public record ProposePersonRequest(
    string FirstName, 
    string LastName, 
    string? NickName,
    string Profession,
    string? Description, 
    string Nationality,
    string? Birthday, 
    string? DeathDate, 
    IEnumerable<string> Tags
);