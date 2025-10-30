using FluentValidation;
using RespectCounter.Application.Auth.Commands;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Application.Auth.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    private readonly IIdentityService _identityService;
    public LoginCommandValidator(IIdentityService userService)
    {
        _identityService = userService;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MustAsync(BeAnExistingUserAsync).WithMessage("Invalid username or email.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is requred");
    }

    private async Task<bool> BeAnExistingUserAsync(string identifier, CancellationToken cancellationToken)
    {
        return await _identityService.ExistsByUsernameOrEmailAsync(identifier, cancellationToken);
    }
}