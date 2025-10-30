using FluentValidation;
using RespectCounter.Application.Reactions.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Reactions.Validators;

public class AddReactionToActivityCommandValidator : AbstractValidator<AddReactionToActivityCommand>
{
    public AddReactionToActivityCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("ActivityId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddReactionToActivityCommand, Domain.Model.Activity>(entityChecker);

        RuleFor(x => x.ReactionType)
            .NotEmpty().WithMessage("ReactionType is required")
            .MustBeAValidEnum<AddReactionToActivityCommand, ReactionType>();

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}