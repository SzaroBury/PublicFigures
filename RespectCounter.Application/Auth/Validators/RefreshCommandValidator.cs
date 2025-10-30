using FluentValidation;
using RespectCounter.Application.Auth.Commands;

namespace RespectCounter.Application.Auth.Validators;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    //private const string RefreshTokenPattern = @"^[a-zA-Z0-9+/]{44}(={0,2})\.[a-fA-F0-9]{32}$";
    private const int BytesLength = 32;

    public RefreshCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("RefreshToken is required.")
            .Must(BeValidRefreshTokenFormat).WithMessage("Invalid RefreshToken format.");
            // .Matches(RefreshTokenPattern).WithMessage("Invalid RefreshToken format.");
    }

    private bool BeValidRefreshTokenFormat(string token)
    {
        var parts = token.Split('.');
        if (parts.Length != 2)
            return false;

        var base64Part = parts[0];
        var guidPart = parts[1];

        if (guidPart.Length != 32 || !Guid.TryParseExact(guidPart, "N", out _))
            return false;
        
        try
        {
            var bytes = Convert.FromBase64String(base64Part);
            
            if (bytes.Length != BytesLength)
                return false;
        }
        catch (FormatException)
        {
            return false;
        }

        return true;
    }
}