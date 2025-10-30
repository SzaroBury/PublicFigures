using FluentValidation;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Tags.Commands;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Tags.Validators;

public class AddTagToActivityCommandValidator : AbstractValidator<AddTagToActivityCommand>
{
    public AddTagToActivityCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("ActivityId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddTagToActivityCommand, Domain.Model.Activity>(entityChecker);

        RuleFor(x => x.TagName)
            .NotEmpty().WithMessage("TagName is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}