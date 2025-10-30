using FluentValidation;
using RespectCounter.Application.Reactions.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Enums;

namespace RespectCounter.Application.Reactions.Validators;

public class AddReactionToPersonCommandValidator : AbstractValidator<AddReactionToPersonCommand>
{
    public AddReactionToPersonCommandValidator(IEntityChecker entityChecker, IIdentityService identityService)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("PersonId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddReactionToPersonCommand, Domain.Model.Activity>(entityChecker);

        RuleFor(x => x.ReactionType)
            .NotEmpty().WithMessage("ReactionType is required")
            .MustBeAValidEnum<AddReactionToPersonCommand, ReactionType>();

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(entityChecker, identityService);
    }
}