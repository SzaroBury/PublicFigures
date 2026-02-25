using FluentValidation;
using RespectCounter.Application.Activity.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Activity.Validators;

public class HideActivityCommandValidator : AbstractValidator<HideActivityCommand>
{
    public HideActivityCommandValidator(IReadOnlyRepository readOnlyRepository, IIdentityService identityService)
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("ActivityId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<HideActivityCommand, Domain.Model.Activity>(readOnlyRepository);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(readOnlyRepository, identityService);
    }
}