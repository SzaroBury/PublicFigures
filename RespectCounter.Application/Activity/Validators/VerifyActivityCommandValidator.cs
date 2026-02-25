using FluentValidation;
using RespectCounter.Application.Activity.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Activity.Validators;

public class VerifyActivityCommandValidator : AbstractValidator<VerifyActivityCommand>
{
    public VerifyActivityCommandValidator(IReadOnlyRepository repository, IIdentityService identityService)
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("ActivityId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<VerifyActivityCommand, Domain.Model.Activity>(repository);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repository, identityService);
    }
}