using FluentValidation;
using RespectCounter.Domain.Enums;
using RespectCounter.Application.Reactions.Commands;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Application.Shared.Extensions;

namespace RespectCounter.Application.Reactions.Validators;

public class AddReactionToActivityCommandValidator : AbstractValidator<AddReactionToActivityCommand>
{
    public AddReactionToActivityCommandValidator(IReadOnlyRepository repo, IIdentityService identityService)
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("ActivityId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingEntityAsync<AddReactionToActivityCommand, Domain.Model.Activity>(repo);

        RuleFor(x => x.ReactionType)
            .NotEmpty().WithMessage("ReactionType is required")
            .MustBeAValidEnum<AddReactionToActivityCommand, ReactionType>();

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustBeAValidGuid()
            .MustBeAnExistingUserAsync(repo, identityService);
    }
}