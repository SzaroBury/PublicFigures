using FluentValidation;
using RespectCounter.Application.Auth.Commands;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Auth.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private IIdentityService _identityService;

    public RegisterCommandValidator(IIdentityService identityService)
    {
        _identityService = identityService;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address format.")
            .MustAsync(BeUniqueEmailAsync).WithMessage("This email is already in use. Please choose a different one.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("The username must be at least 3 characters long.")
            .MustAsync(BeUniqueUsernameAsync).WithMessage("This username is already taken. Please choose a different one.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Your password must be at least 8 characters long.")
            .MaximumLength(50).WithMessage("Your password cannot exceed 50 characters.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Password confirmation is requred.")
            .Equal(x => x.Password).WithMessage("The password confirmation does not match the password.");
    }

    private async Task<bool> BeUniqueEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _identityService.IsUniqueEmailAdressAsync(email, cancellationToken);
    }

    private async Task<bool> BeUniqueUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _identityService.IsUniqueUsernameAsync(username, cancellationToken);
    }
}