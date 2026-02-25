using FluentValidation;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Application.Tag.Commands;

namespace RespectCounter.Application.Tag.Validators;

public class AddTagToActivityCommandValidator : AbstractValidator<AddTagToActivityCommand>
{
    public AddTagToActivityCommandValidator(IReadOnlyRepository repo, IIdentityService identityService)
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("ActivityId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddTagToActivityCommand, Domain.Model.Activity>(repo);

        RuleFor(x => x.TagName)
            .NotEmpty().WithMessage("TagName is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repo, identityService);
    }
}