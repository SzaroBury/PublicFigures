using FluentValidation;
using RespectCounter.Application.Auth.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Auth.Validators;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator(IReadOnlyRepository readOnlyRepository, IIdentityService identityService)
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(readOnlyRepository, identityService);
    }
}