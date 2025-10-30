namespace RespectCounter.API.Requests;

public record RegisterRequest(
    string Email,
    string Username,
    string Password,
    string ConfirmPassword
);